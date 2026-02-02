namespace MusicLibrary.Application.Requests.Music;

public sealed record AddMusicToAlbumRequest
(
    Guid ArtistId,
    Guid AlbumId,
    Guid MusicId
);