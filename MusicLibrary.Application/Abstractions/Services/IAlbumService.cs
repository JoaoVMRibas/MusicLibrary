using MusicLibrary.Application.Requests.Album;
using MusicLibrary.Application.Responses.DTOs;

namespace MusicLibrary.Application.Abstractions.Services;

public interface IAlbumService
{
    Task<AlbumDto> CreateAlbumAsync(CreateAlbumRequest request);
    Task<AlbumDto?> GetAlbumByIdAsync(GetAlbumByIdRequest request);
    Task<IReadOnlyCollection<AlbumDto>> GetAlbumsByArtist(GetAlbumsByArtistRequest request);
    Task DeleteAlbumAsync(DeleteAlbumRequest request);
}
