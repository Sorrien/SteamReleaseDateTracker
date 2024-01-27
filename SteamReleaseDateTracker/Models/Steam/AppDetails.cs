using System.Text.Json.Serialization;

namespace SteamReleaseDateTracker.Models.Steam
{
    public class AppDetails
    {
        [JsonPropertyName("steam_appid")]
        public int SteamAppId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("release_date")]
        public ReleaseDate ReleaseDate { get; set; }
    }
}
