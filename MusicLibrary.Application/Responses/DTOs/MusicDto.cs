namespace MusicLibrary.Application.Responses.DTOs;

public sealed record MusicDto
(
    Guid Id,
    string Name,
    TimeSpan Duration
);
