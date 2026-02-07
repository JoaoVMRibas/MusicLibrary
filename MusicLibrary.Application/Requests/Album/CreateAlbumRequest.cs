namespace MusicLibrary.Application.Requests.Album;

public sealed record CreateAlbumRequest
(
    Guid ArtistId,
    string Name
);