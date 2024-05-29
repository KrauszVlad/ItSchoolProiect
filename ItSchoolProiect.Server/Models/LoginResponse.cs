using System.Text.Json.Serialization;

namespace ItSchoolProiect.Server.Models
{
    public class LoginResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;

        [JsonPropertyName("expire")]
        public DateTime Expire { get; set; } = DateTime.UtcNow;
    }
}
