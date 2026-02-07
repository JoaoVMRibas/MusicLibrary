namespace MusicLibrary.WebAPI.Models;

public sealed record CreateMusicBody(string Name,int DurationInSeconds)
{
    public TimeSpan GetDuration() => TimeSpan.FromSeconds(DurationInSeconds);
}
