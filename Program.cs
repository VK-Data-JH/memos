using MEMOS.Classes.Tasks;
using MEMOS.data;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace MEMOS
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Duplicity - úkol 1 \n");
            new DuplicityTask().GetResult();


            Console.WriteLine("\n Tree - úkol 2 \n");
            string nameCrew = "Worfson of Mog";
            string planetName = "Kashyyyk";

            new TreeTask(new DataTreeTask().GetItems()).GetSubsidaries(nameCrew);
            new TreeTask(new DataTreeTask().GetItems()).GetIlledPersons(nameCrew);

            Console.WriteLine("\n API  - úkol 3 \n Čekejte, prosím \n");
            string apitask = await new APITask(planetName).GetResult();
            if (!string.IsNullOrEmpty(apitask))
            {
                JsonDocument json = JsonDocument.Parse(apitask);
            }
        }
    }        
}
