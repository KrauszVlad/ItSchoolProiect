﻿using System.Text.Json.Serialization;

namespace ItSchoolProiect.Server.Models
{
    public class LoginRequest
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty; 

    }
}
