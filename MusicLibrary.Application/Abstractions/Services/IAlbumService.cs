using MusicLibrary.Application.Requests.Album;
using MusicLibrary.Application.Responses.DTOs;

namespace MusicLibrary.Application.Abstractions.Services;

public interface IAlbumService
{
    Task<AlbumDto> CreateAlbumAsync(CreateAlbumRequest request);
    Task<AlbumDto?> GetAlbumByIdAsync(Guid artistId, Guid albumId);
    Task<IReadOnlyCollection<AlbumDto>> GetAlbumsByArtist(Guid artistId);
    Task DeleteAlbumAsync(Guid artistId, Guid albumId);
}