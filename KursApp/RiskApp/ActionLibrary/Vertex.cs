﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiskApp
{
    public class Vertex
    {
        double x;
        double val;
        public double X
        {
            get => x;
            set { x = Double.Parse($"{value:f3}"); }
        }
        public double Y { get; }
        public double Value
        {
            get => val;
            set { val = double.Parse($"{value:f3}"); }
        }
        public int ID { get;}
        public int IDParent { get; }
        public double Cost { get; }
        public double Probability { get; }
        public string Description { get; }

        public Vertex(double x, double y, double cost, double probability, int idParent, string description)
        {
            X = x;
            Y = y;
            Cost = cost;
            Probability = probability;
            IDParent = idParent;
            Description = description;
        }
        public Vertex(double x, double y, int idParent, string description)
        {
            X = x;
            Y = y;
            IDParent = idParent;
            Description = description;
        }

        public Vertex( double x, double y, int id, int idParent, string description)
        {
            X = x;
            Y = y;
            ID = id;
            IDParent = idParent;
            Description = description;
        }

        public Vertex(double x, double y, double cost, double probability, int id, int parentId, string description)
        {
            X = x;
            Y = y;
            Cost = cost;
            Probability = probability;
            ID = id;
            IDParent = parentId;
            Description = description;
        }
    }
}
