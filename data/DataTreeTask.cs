using MEMOS.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEMOS.data
{
    public class DataTreeTask
    {
        public List<TreeItem> GetItems()
        {
            return items;
        }

         private readonly List<TreeItem> items = new List<TreeItem>
         {
              new TreeItem { Id= 1, Name= "Jean-Luc Picard", Position= "Captain", ParentId= 0 },
              new TreeItem  { Id= 2, Name= "William Riker", Position= "First Officer", ParentId= 1 },
              new TreeItem  { Id= 3, Name= "Mr.Data", Position= "Second Officer / Operations", ParentId= 7 },
              new TreeItem  { Id= 4, Name= "Worfson of Mog", Position= "Chief of Security / Tactical", ParentId= 2 },
              new TreeItem  { Id= 5, Name= "Beverly Crusher", Position= "Chief Medical Officer", ParentId= 2 },
              new TreeItem  { Id= 6, Name= "Alyssa Ogawa", Position= "Nurse", ParentId= 5 },
              new TreeItem  { Id= 7, Name= "Jordi La Forge", Position= "Chief Engineer", ParentId= 1 },
              new TreeItem  { Id= 8, Name= "Deanna Troi", Position= "Counselor", ParentId= 1 },
              new TreeItem  { Id= 9, Name= "Miles O'Brien", Position= "Transporter Chief", ParentId= 7 },
              new TreeItem  { Id= 10, Name= "Reginald Barclay", Position= "Systems Diagnostic Engineer", ParentId= 8 },
              new TreeItem  { Id= 11, Name= "Tasha Yar", Position= "Chief of Security (Season 1)", ParentId= 4 },
              new TreeItem  { Id= 14, Name= "Lwaxana Troi", Position= "Ambassador / Counselor's Mother", ParentId= 8 },
              new TreeItem  { Id= 16, Name= "Alexander Rozhenko", Position= "Worf's Son", ParentId= 18 },
              new TreeItem  { Id= 17, Name= "Wesley Crusher", Position= "Acting Ensign / Navigator", ParentId= 5 },
               new TreeItem  { Id= 18, Name= "K'Ehleyr", Position= "Acting Ensign / Navigator", ParentId= 4 },
               new TreeItem  { Id= 19, Name= "Julian Bashir", Position= "Acting Ensign / Navigator", ParentId= 6 },
               new TreeItem  { Id= 20, Name= "Guinan", Position= "Acting Ensign / Navigator", ParentId= 2 }
         };
    }

}
