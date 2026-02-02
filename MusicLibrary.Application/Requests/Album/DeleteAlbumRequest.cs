namespace MusicLibrary.Application.Requests.Album;

public sealed record DeleteAlbumRequest
(
    Guid ArtistId, 
    Guid AlbumId
);
