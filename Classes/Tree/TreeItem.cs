﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEMOS.Classes
{
    public class TreeItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }

        public string Position { get; set; }
        public List<int> Childrens { get; set; }
    }
}
