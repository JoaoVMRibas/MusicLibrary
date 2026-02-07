using MusicLibrary.Application.Requests.Music;
using MusicLibrary.Application.Responses.DTOs;

namespace MusicLibrary.Application.Abstractions.Services;

public interface IMusicService
{
    Task<MusicDto> CreateMusicAsync(CreateMusicRequest request);
    Task<MusicDto?> GetMusicByIdAsync(Guid artistId,Guid id);
    Task<IReadOnlyCollection<MusicDto>> GetMusicsByArtistAsync(Guid artistId);
    Task AddMusicToAlbumAsync(Guid artistId, Guid albumId, Guid id);
    Task DeleteMusicAsync(Guid artistId, Guid id);
}