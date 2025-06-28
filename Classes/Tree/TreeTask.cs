using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace MEMOS.Classes.Tasks
{
    internal class TreeTask
    {        
        private Dictionary<int, TreeItem> ParentChildData = new Dictionary<int, TreeItem>();
        private Queue<TreeItem> SubsidiariesQueue = new Queue<TreeItem>();
        public TreeTask(List<TreeItem> crew) 
        {
           foreach(var person in crew)
            {
                Boolean exist =this.ParentChildData.ContainsKey(person.Id);
                if(!exist)
                {
                    this.ParentChildData.Add(person.Id, person);
                }
            }
        }

        public void GetIlledPersons(string name)
        {
            GetQueue(name);
            Dictionary<string, TreeItem> illCrew = new Dictionary<string, TreeItem>();
            do
            {
                if (this.SubsidiariesQueue.Count > 0)
                {
                    TreeItem subsid = this.SubsidiariesQueue.Dequeue();
                    Boolean exist =illCrew.ContainsKey(subsid.Name);
                    if (subsid.Id == 1)
                    {
                        illCrew.Add(subsid.Name, subsid);
                        this.SubsidiariesQueue.Clear();
                    }
                    else if (!exist && subsid.Id != 1 && subsid.Name!=name)
                    {
                        illCrew.Add(subsid.Name, subsid);
                        GetQueue(subsid.Name);
                    }
                }
            }
            while (this.SubsidiariesQueue.Count > 0);
            Console.WriteLine();
            Console.WriteLine($"Potencionálně nemocní od {name}");
           foreach( var ill in illCrew)
            {
                Console.WriteLine($"{ill.Value.Name} - {ill.Value.Position}");
            }
        }
        public void GetSubsidaries(string name)
        {
            GetSubsidiariesQueue(name);
            Console.WriteLine($"Podřízení {name}");
            do
                {
                    if(this.SubsidiariesQueue.Count>0)
                    {
                        TreeItem subsid = this.SubsidiariesQueue.Dequeue();
                        Console.WriteLine($"{subsid.Name} - {subsid.Position}- Nadřízený {this.ParentChildData.First(p=>p.Key== subsid.ParentId).Value.Name}");
                        GetSubsidiariesQueue(subsid.Name);
                    }
            }
            while(this.SubsidiariesQueue.Count > 0 );
        }
        private void GetSubsidiariesQueue(string name)
        {           
            int personId = this.ParentChildData.FirstOrDefault(p => p.Value.Name == name).Key;
            foreach (var person in this.ParentChildData)
            { if (person.Value.ParentId == personId)
                {
                    this.SubsidiariesQueue.Enqueue(person.Value);
                }
            }
        }
        private void GetQueue(string name)
        {
            int personId = this.ParentChildData.FirstOrDefault(p => p.Value.Name == name).Value.Id;
            int chiefId = this.ParentChildData.FirstOrDefault(p => p.Value.Name == name).Value.ParentId;
            foreach (var person in this.ParentChildData)
            { if (person.Value.ParentId == personId || person.Value.Id==chiefId)
                {
                    this.SubsidiariesQueue.Enqueue(person.Value);
                }
            }
        }

    }
}
