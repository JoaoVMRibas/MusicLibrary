namespace MusicLibrary.Application.Requests.Music;

public sealed record DeleteMusicRequest
(
    Guid ArtistId,
    Guid MusicId
);