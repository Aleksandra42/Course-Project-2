﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;

namespace RiskApp
{
    public class Risk
    {
        private int status;
        private int idUser;
        private double probability;
        private double influence;

        public Point point;
        
        public int ID { get; }
        public string RiskName { get; }
        public string Source { get; }
        public string Effects { get; }
        public string Solution { get; }
        public string Type { get; }
        public string OwnerLogin { get; set; }
        public int Status
        {
            get => status;
            set { status = value; }
        }
        public int IdUser
        {
            get => idUser;
            set
            {
                idUser = value;
            }
        }
        public double Probability
        {
            get => probability;
            set
            {
                if (value > 1 || value < 0)
                    throw new ArgumentException("Probability must be in the interval (0; 1)");

                probability = value;
            }
        }
        public double Influence
        {
            get => influence;
            set
            {
                if (value > 1 || value < 0)
                    throw new ArgumentException("Influence must be in the interval (0; 1)");

                influence = value;
            }
        }
        public double Rank { get => Influence * Probability; }
        public Risk(string riskName, string source, string effects, string type, string description, int id)
        {
            RiskName = riskName;
            Source = source;
            Effects = effects;
            Type = type;
            Solution = description;
            ID = id;
        }

        public Risk(int id, int status, double probability, double influence, 
            string riskName, string source, string effects,
            string type, string description) 
        {
            ID = id;
            Status = status;
            Probability = probability;
            Influence = influence;
            RiskName = riskName;
            Source = source;
            Effects = effects;
            Type = type;
            Solution = description;
        }
        public Risk(int id, int status, int idUser, double probability, double influence, 
            string riskName, string source, string effects,
            string type, string ownerLogin, string description)
        {
            ID = id;
            Status = status;
            IdUser = idUser;
            Probability = probability;
            Influence = influence;
            RiskName = riskName;
            Source = source;
            Effects = effects;
            Type = type;
            OwnerLogin = ownerLogin;
            Solution = description;
        }
        
        public override string ToString() => $"Name: {RiskName}\nType: {Type}";
    }
}
