﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiskApp
{
    public class Project
    {
        public int ID { get; }
        public string Name { get;} 
        public string Owner { get; }
        public string Type { get; }
        public Project(string name, string owner, string type)
        {
            Name = name;
            Owner = owner;
            Type = type;
        }

        public Project(int id, string name, string owner, string type)
        {
            ID = id;
            Name = name;
            Owner = owner;
            Type = type;
        }
        
        public override string ToString() => $"{Name}\nOwner: {Owner}\t {Type}";
    }
}
