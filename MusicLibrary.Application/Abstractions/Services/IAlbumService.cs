using MusicLibrary.Application.Requests;
using MusicLibrary.Application.Responses.DTOs;
using MusicLibrary.Domain.Entities;

namespace MusicLibrary.Application.Abstractions.Services;

public interface IAlbumService
{
    Task<AlbumDto> CreateAlbumAsync(CreateAlbumRequest request);
    Task<AlbumDto?> GetAlbumByIdAsync(Guid artistId,Guid albumId);
    Task<IReadOnlyCollection<Album>> GetAlbumsByArtist(Guid artistId);
    Task DeleteAlbumAsync(Guid artistId,Guid albumId);
}
