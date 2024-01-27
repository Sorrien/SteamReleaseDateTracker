using SteamReleaseDateTracker;
using SteamReleaseDateTracker.Models.Application;

var appIds = File.ReadLines("appids.txt");

var httpclient = new HttpClient { BaseAddress = new Uri("https://store.steampowered.com/api/") };
var apiTasks = appIds.Select(appId => SteamAPIHelper.GetAppDetails(appId, httpclient));

var responses = await Task.WhenAll(apiTasks);

var games = responses.Select(response =>
{
    var isDate = DateOnly.TryParse(response.Data.ReleaseDate.Date, out var date);

    return new GameData
    {
        AppId = response.Data.SteamAppId,
        Name = response.Data.Name.Replace("\u2122", ""),
        IsReleased = !response.Data.ReleaseDate.ComingSoon,
        ReleaseDate = isDate ? date : null,
        ReleaseDateString = isDate ? date.ToString() : response.Data.ReleaseDate.Date
    };
});

var fileSafeDateTimeNowString = DateTime.Now.ToString().Replace("/", "_").Replace(" ", "_").Replace(":", "_");
ApplicationHelper.PrintGames(games);

var previousGameData = ApplicationHelper.GetMostRecentGameData();
ApplicationHelper.PrintNotifications(games, previousGameData);

ApplicationHelper.SaveGameData(games);