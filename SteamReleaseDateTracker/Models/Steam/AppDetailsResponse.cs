using System.Text.Json.Serialization;

namespace SteamReleaseDateTracker.Models.Steam
{
    public class AppDetailsResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("data")]
        public AppDetails Data { get; set; }
    }
}
