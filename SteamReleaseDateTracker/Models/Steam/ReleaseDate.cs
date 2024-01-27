using System.Text.Json.Serialization;

namespace SteamReleaseDateTracker.Models.Steam
{
    public class ReleaseDate
    {
        [JsonPropertyName("coming_soon")]
        public bool ComingSoon { get; set; }
        [JsonPropertyName("date")]
        public string Date { get; set; }
    }
}
