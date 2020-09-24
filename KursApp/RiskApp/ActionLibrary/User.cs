﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiskApp
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; }
        public string Login { get; set; }
        public string Password { get; }
        public string Position { get; }
        public User(int id, string name, string login, string password, string position)
        {
            ID = id;
            Name = name;
            Login = login;
            Password = password;
            Position = position;
        }
        public User() { }

        public override string ToString() => $"User's ID: {ID},\nName:{Name},\nPosition: {Position}";
    }
}
