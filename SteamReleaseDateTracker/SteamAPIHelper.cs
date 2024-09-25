using SteamReleaseDateTracker.Models.Steam;
using System.Text.Json;

namespace SteamReleaseDateTracker
{
    public static class SteamAPIHelper
    {
        public static async Task<(string, AppDetailsResponse)> GetAppDetails(string appId, HttpClient httpclient)
        {
            //https://wiki.teamfortress.com/wiki/User:RJackson/StorefrontAPI
            var httpResponse = await httpclient.GetAsync($"appdetails/?appids={appId}&l=english");

            if (httpResponse.IsSuccessStatusCode)
            {
                var response = await JsonSerializer.DeserializeAsync<Dictionary<string, AppDetailsResponse>>(await httpResponse.Content.ReadAsStreamAsync());

                if (response.Values.Count == 0 || response.Values.First().Success == false)
                {
                    return (appId, null);
                }
                else
                {
                    return (appId, response.Values.First());
                }
            }
            else
            {
                return (appId, null);
            }
        }
    }
}
