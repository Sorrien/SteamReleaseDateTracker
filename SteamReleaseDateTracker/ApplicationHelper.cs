using SteamReleaseDateTracker.Models.Application;
using System.Text;
using System.Text.Json;

public static class ApplicationHelper
{
    public const string FilePrefix = "gamedata";

    public static List<GameData>? GetMostRecentGameData()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var files = Directory.GetFiles(currentDirectory);
        var gameDataFiles = files.Where(x => x.Contains(FilePrefix));

        var mostRecentFile = gameDataFiles.FirstOrDefault();
        var mostRecentFileDate = new DateTime();
        foreach (var gameDataFile in gameDataFiles)
        {
            var fileCreatedDate = File.GetCreationTimeUtc(gameDataFile);

            if (fileCreatedDate > mostRecentFileDate)
            {
                mostRecentFileDate = fileCreatedDate;
                mostRecentFile = gameDataFile;
            }
        }
        List<GameData> previousGameData;

        if (mostRecentFile == null)
        {
            previousGameData = null;
        }
        else
        {
            var gameDataString = File.ReadAllText(mostRecentFile);
            previousGameData = JsonSerializer.Deserialize<List<GameData>>(gameDataString);
        }

        return previousGameData;
    }

    public static string GetFileSafeDateTimeNowString()
    {
        return DateTime.Now.ToString().Replace("/", "_").Replace(" ", "_").Replace(":", "_");
    }

    public static void PrintGames(IEnumerable<GameData> games)
    {
        var fileSafeDateTimeNowString = GetFileSafeDateTimeNowString();

        var orderedGames = games.OrderBy(game => game.ReleaseDate);

        var monthDict = new Dictionary<int, List<GameData>>();

        for (var i = 1; i < 13; i++)
        {
            monthDict.Add(i, []);
        }

        foreach (var game in orderedGames)
        {
            if (game.ReleaseDate.HasValue)
            {
                var releaseDate = game.ReleaseDate.Value;
                monthDict[releaseDate.Month].Add(game);
            }
        }

        foreach (var month in monthDict)
        {
            var monthList = month.Value;
            if (monthList.Count > 0)
            {
                var monthName = new DateTime(2024, month.Key, 1).ToString("MMM");
                Console.WriteLine($"{monthName}:");
                foreach (var game in monthList)
                {
                    Console.WriteLine($"{game.Name} ({StringHelper.AddOrdinal(game.ReleaseDate.Value.Day)})");
                }
                Console.WriteLine(string.Empty);
            }
        }

        var gamesWithYearReleaseDates = games.Where(x => int.TryParse(x.ReleaseDateString, out var _)).OrderBy(x => x.ReleaseDateString);

        var gamesWithQuarterReleaseDates = games.Where(x => x.ReleaseDateString.ToLower().StartsWith('q')).OrderBy(x => x.ReleaseDateString);

        Console.WriteLine("Quarters:");
        foreach (var game in gamesWithQuarterReleaseDates)
        {
            Console.WriteLine($"{game.Name} ({game.ReleaseDateString})");
        }
        Console.WriteLine(string.Empty);

        Console.WriteLine("Only release year is known:");
        foreach (var game in gamesWithYearReleaseDates)
        {
            Console.WriteLine($"{game.Name} ({game.ReleaseDateString})");
        }
    }

    public static void PrintNotifications(IEnumerable<GameData> games, IEnumerable<GameData> previousGames)
    {
        Console.WriteLine(string.Empty );
        var notifications = new List<string>();
        if (previousGames != null)
        {
            foreach (var game in games)
            {
                var previousGame = previousGames.FirstOrDefault(x => x.AppId == game.AppId);
                if (previousGame is not null && (previousGame.ReleaseDateString != game.ReleaseDateString))
                {
                    notifications.Add($"New release date found for {game.Name}! {game.ReleaseDateString}");
                }
            }
        }

        if (notifications.Count > 0)
        {
            var sb = new StringBuilder();

            foreach (var notification in notifications)
            {
                Console.WriteLine(notification);
                sb.AppendLine(notification);
            }
            var result = sb.ToString();
            File.WriteAllText($"notification_{GetFileSafeDateTimeNowString()}.txt", result);
        }
    }

    public static void SaveGameData(IEnumerable<GameData> games)
    {
        var newResultString = JsonSerializer.Serialize(games);
        File.WriteAllText($"{ApplicationHelper.FilePrefix}_{GetFileSafeDateTimeNowString()}.json", newResultString);
    }
}