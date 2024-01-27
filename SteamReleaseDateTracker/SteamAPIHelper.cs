using SteamReleaseDateTracker.Models.Steam;
using System.Text.Json;

namespace SteamReleaseDateTracker
{
    public static class SteamAPIHelper
    {
        public static async Task<AppDetailsResponse> GetAppDetails(string appId, HttpClient httpclient)
        {
            //https://wiki.teamfortress.com/wiki/User:RJackson/StorefrontAPI
            var httpResponse = await httpclient.GetAsync($"appdetails/?appids={appId}");

            if (httpResponse.IsSuccessStatusCode)
            {
                var response = await JsonSerializer.DeserializeAsync<Dictionary<string, AppDetailsResponse>>(await httpResponse.Content.ReadAsStreamAsync());

                return response.Values.First();
            }
            else
            {
                return null;
            }
        }
    }
}
