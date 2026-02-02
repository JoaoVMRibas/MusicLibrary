namespace MusicLibrary.Application.Requests.Artist;

public sealed record UpdateArtistRequest
(
    Guid ArtistId, 
    string Name
);