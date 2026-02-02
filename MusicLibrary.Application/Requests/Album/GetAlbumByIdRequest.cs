namespace MusicLibrary.Application.Requests.Album;

public sealed record GetAlbumByIdRequest
(
    Guid ArtistId, 
    Guid AlbumId
);
