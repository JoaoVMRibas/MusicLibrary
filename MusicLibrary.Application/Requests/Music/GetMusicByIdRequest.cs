namespace MusicLibrary.Application.Requests.Music;

public sealed record GetMusicByIdRequest
(
    Guid ArtistId,
    Guid MusicId
);