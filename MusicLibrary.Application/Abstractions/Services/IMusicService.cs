using MusicLibrary.Application.Requests.Music;
using MusicLibrary.Application.Responses.DTOs;

namespace MusicLibrary.Application.Abstractions.Services;

public interface IMusicService
{
    Task<MusicDto> CreateMusicAsync(CreateMusicRequest request);
    Task<MusicDto?> GetMusicByIdAsync(GetMusicByIdRequest request);
    Task<IReadOnlyCollection<MusicDto>> GetMusicsByArtistAsync(GetMusicsByArtistRequest request);
    Task AddMusicToAlbumAsync(AddMusicToAlbumRequest request);
    Task DeleteMusicAsync(DeleteMusicRequest request);
}
