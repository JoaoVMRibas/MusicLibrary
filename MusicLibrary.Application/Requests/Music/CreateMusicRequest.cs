namespace MusicLibrary.Application.Requests.Music;

public sealed record CreateMusicRequest
(
    Guid ArtistId,
    string Name,
    TimeSpan Duration
);
