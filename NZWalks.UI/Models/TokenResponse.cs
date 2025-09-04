using System.Text.Json.Serialization;

namespace NZWalks.UI.Models
{
    public class TokenResponse
    {
        [JsonPropertyName("jwtToken")]
        public string Token { get; set; }
    }
}
