namespace MusicLibrary.Application.Requests;

public sealed record UpdateArtistRequest(Guid Id, string Name);