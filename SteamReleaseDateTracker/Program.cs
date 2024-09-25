using SteamReleaseDateTracker;
using SteamReleaseDateTracker.Models.Application;
using SteamReleaseDateTracker.Models.Steam;

var appIds = await File.ReadAllLinesAsync("appids.txt");

var httpclient = new HttpClient { BaseAddress = new Uri("https://store.steampowered.com/api/") };

var batches = appIds.Chunk(5);

var responses = new List<(string, AppDetailsResponse)>();
foreach (var batch in batches)
{
    var tasks = batch.Select(appId => SteamAPIHelper.GetAppDetails(appId, httpclient));
    responses.AddRange(await Task.WhenAll(tasks));
    Thread.Sleep(1000);
}

var previousGameData = ApplicationHelper.GetMostRecentGameData();

var games = responses.Select(data =>
{
    var (appId, response) = data;

    if (response is null)
    {
        var previousData = previousGameData.FirstOrDefault(x => x.AppId.ToString() == appId);
        if (previousData is null)
        {
            return null;
        }
        else
        {
            return previousData;
        }
    }
    else
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
    }
}).Where(game => game is not null);

var fileSafeDateTimeNowString = DateTime.Now.ToString().Replace("/", "_").Replace(" ", "_").Replace(":", "_");
ApplicationHelper.PrintGames(games);

ApplicationHelper.PrintNotifications(games, previousGameData);

ApplicationHelper.SaveGameData(games);