﻿namespace ControlAcceso.Data.Model
{
    public class UserModel
    {
        public int? Id { get; set; }
        
        public string? Username { get; set; }
        
        public string? Email { get; set; }

        public string? FirstName { get; set; }

        public string? SecondName { get; set; }

        public string? Lastname { get; set; }

        public string? SecondLastname { get; set; }
        
        public string? Password { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }
        
        public string? RoleId { get; set; }
        
        public string? Role { get; set; }
    }
}