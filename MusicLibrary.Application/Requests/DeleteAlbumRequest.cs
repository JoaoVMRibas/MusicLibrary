namespace MusicLibrary.Application.Requests;

public sealed record DeleteAlbumRequest
(
    Guid ArtistId, 
    Guid AlbumId
);
