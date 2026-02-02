namespace MusicLibrary.Application.Requests;

public sealed record CreateAlbumRequest
(
    Guid ArtistId,
    string Name
);
