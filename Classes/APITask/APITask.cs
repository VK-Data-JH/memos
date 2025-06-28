

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace MEMOS.Classes.Tasks
{
    public class APITask
    {
        private string Name;
        private readonly HttpClient _client;

        private readonly Dictionary<string,string> queryParameters = new Dictionary<string, string>()
            {
                {"expanded","true" }
            };
        private readonly string url = "www.swapi.tech";

        private readonly string[] paths = new string[]
            {
               "api/planets",
               "api/people",
               "api/starships"
            };

        public APITask(string Name)           
        {
            _client = new HttpClient();
            this.Name = Name;
        }

        public async Task<string> GetResult ()
        {          
            string name = this.Name;

            ///planet
            IEnumerable<JsonElement> planet = await FetchData(url, paths[0], GetPlanetQuery(queryParameters, name),true);
            if (!planet.Any())
            { 
                Console.WriteLine($"Žádná planeta s názvem {name} nenalezena.");
                return null;
            }
            planet.First().GetProperty("properties").TryGetProperty("url",out JsonElement planetUrl);

            //pilots
            Dictionary<string,Pilot> pilotsFromPlanet = await GetPilotsFromPlanet(planetUrl.GetString());
            if (!pilotsFromPlanet.Any())
            {
                Console.WriteLine("Žádný pilot nenalezen");
                return null;
            }

            //starships
            List<JsonElement> starshipsPilotedByPilots = await GetStarhipsPiloted(pilotsFromPlanet);
            if (!starshipsPilotedByPilots.Any())
            {
                Console.WriteLine("Žádná odpovídající loď nenalezena");
                return null;
            }

            Console.WriteLine($"Lodě pilotované lidmi narozenými na {name}");
            foreach (var starship in starshipsPilotedByPilots.Distinct().ToList())
            {
                if(starship.GetProperty("properties").GetProperty("name").ValueKind==JsonValueKind.String)
                {
                string starshipName = starship.GetProperty("properties").GetProperty("name").GetString();
                    Console.WriteLine($"{starshipName}");
                }
            }
            return JsonSerializer.Serialize(starshipsPilotedByPilots);
        }

        private async Task<List<JsonElement>> GetStarhipsPiloted(Dictionary<string,Pilot> pilots)
        {
            List<JsonElement> starshipsPiloted = new List<JsonElement>();
            IEnumerable<JsonElement> starships = await FetchData(url, paths[2], queryParameters, false);

            foreach (var starship in starships)            {
                starship.GetProperty("properties").TryGetProperty("pilots", out JsonElement pilotList);

                foreach(var pilot in pilotList.EnumerateArray())
                {
                    if(pilots.Any(p=>p.Value.Url==pilot.GetString()))
                      {
                        starshipsPiloted.Add(starship);
                      }
                }
            }
            return starshipsPiloted;
        }

        private async Task<Dictionary<string,Pilot>> GetPilotsFromPlanet (string planetUrl)
        {           
            Dictionary<string,Pilot> pilotsFromPlanet = new Dictionary<string,Pilot>();
            IEnumerable<JsonElement> pilots =await FetchData(url, paths[1], queryParameters, false);
            foreach (var pilot in pilots)
            {
                pilot.TryGetProperty("properties",out JsonElement props);
                string name = props.GetProperty("name").GetString();
                string homeworld = props.GetProperty("homeworld").GetString();

                if (homeworld == planetUrl)
                {
                    pilotsFromPlanet.Add(name, new Pilot
                    {
                        Name = name,
                        Homeworld = homeworld,
                        Url = props.GetProperty("url").GetString()
                    });
                }
            }
            return pilotsFromPlanet;
        }
        public Dictionary<string, string> GetPlanetQuery(Dictionary<string, string> queryParameters, string name)
        {
            Dictionary<string, string> planetQuery = new Dictionary<string, string>();
            foreach (var key in queryParameters.Keys)
            {
                string value = queryParameters[key];
                planetQuery[key] = value;
            }
            planetQuery.Add("name", name);
            return planetQuery;            
        }
        private async Task<IEnumerable<JsonElement>> FetchData(string host,string path,Dictionary<string, string> queryParameters,Boolean isDetail)
        {
            List<JsonElement> resultsList = new List<JsonElement>();
            var uri = new UriBuilder()
            {
                Host = host,
                Query =GetQuery(queryParameters),
                Path=$"/{path}/",
                Scheme="https"  
            };

            JsonElement firstRootElement = await FetchPage(uri.Uri.ToString());
            
            GetResultList(resultsList, firstRootElement,isDetail);

            if(firstRootElement.ValueKind==JsonValueKind.Undefined)
            {
                return resultsList;
            }

            if (firstRootElement.TryGetProperty("next", out JsonElement nextpage) && nextpage.ValueKind == JsonValueKind.String)
            {
                string next = nextpage.GetString();
                do
                {
                    JsonElement nextRootElement = await FetchPage(next);
                    next = nextRootElement.GetProperty("next").GetString();
                    GetResultList(resultsList, nextRootElement, isDetail);

                }
                while (!string.IsNullOrEmpty(next));
            }
            return resultsList;
        }
        private async Task<JsonElement> FetchPage (string url)
        {
            HttpResponseMessage fetchdata = await _client.GetAsync(url);
            if (!fetchdata.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error while data fetching {fetchdata.StatusCode}");
                throw new Exception($"error while data fetching {url}");
            }
            else
            {
                string contentType = fetchdata.Content.Headers.ContentType?.MediaType ?? "";

                if (!contentType.Contains("application/json"))
                {
                    string errorBody = await fetchdata.Content.ReadAsStringAsync();
                    Console.WriteLine("Neplatný obsah z API:");
                    Console.WriteLine(errorBody.Substring(0, Math.Min(200, errorBody.Length))); // vypiš max 200 znaků
                    return new JsonElement();
                }
                string jsonString = await fetchdata.Content.ReadAsStringAsync();
                if (JsonDocument.Parse(jsonString) != null)
                {
                    return JsonDocument.Parse(jsonString).RootElement;
                }
            }
            return new JsonElement();
        }

        private JsonElement GetProperty(JsonElement json,string property)
        {         
            if(json.ValueKind==JsonValueKind.Undefined)
            {
                Console.WriteLine("Žádná data nebyla obdržena z API");
                return new JsonElement();
            }
            Boolean exist= json.TryGetProperty(property, out JsonElement result);
            return exist ? result : new JsonElement { };            
        }

        private void GetResultList (List<JsonElement> resultList,JsonElement newJson,Boolean isDetail)
        {
            List<JsonElement> jsonElements = resultList;
            string propertyName = isDetail ? "result" : "results";
            JsonElement newResults = GetProperty(newJson,propertyName);

            if (newResults.ValueKind == JsonValueKind.Undefined)
            {
                return;
            }

            foreach (JsonElement item in newResults.EnumerateArray())
            {
                jsonElements.Add(item);
            }
        }
        private static string GetQuery(Dictionary<string, string> queryParameters)
        {
            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
            foreach(var param in queryParameters)
            {
                queryString.Add(param.Key, param.Value);
            }
            return queryString.ToString();
        }
    }
}