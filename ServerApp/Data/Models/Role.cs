﻿namespace ServerApp.Data.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public List<User> Users { get; set; } = new List<User>();
    }
}
