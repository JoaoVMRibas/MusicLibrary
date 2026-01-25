namespace MusicLibrary.Application.Responses.DTOs;

public sealed record AlbumDto
(
    Guid Id,
    string Name,
    TimeSpan Duration
);
