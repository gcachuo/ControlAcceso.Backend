﻿using System.Text.Json.Serialization;

namespace ControlAcceso.Endpoints.Users
{
    public class UserRequest
    {
        public string? Email { get; set; }

        public string? FirstName { get; set; }

        public string? SecondName { get; set; }

        public string? FirstSurname { get; set; }

        public string? SecondSurname { get; set; }

        public string? Password { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string? RoleId { get; set; }

    }

    public class LoginRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        [JsonPropertyName("user_agent")]
        public string? UserAgent { get; set; }
    }
}