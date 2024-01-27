namespace SteamReleaseDateTracker.Models.Application
{
    public class GameData
    {
        public int AppId { get; set; }
        public string Name { get; set; }
        public DateOnly? ReleaseDate { get; set; }
        public bool IsReleased { get; set; }
        public string ReleaseDateString { get; set; }
    }
}
