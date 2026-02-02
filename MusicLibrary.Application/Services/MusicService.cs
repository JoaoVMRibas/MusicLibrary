using MusicLibrary.Application.Abstractions.Services;
using MusicLibrary.Application.Requests.Music;
using MusicLibrary.Application.Responses.DTOs;

namespace MusicLibrary.Application.Services;

public class MusicService : IMusicService
{

    public async Task<MusicDto> CreateMusicAsync(CreateMusicRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<MusicDto?> GetMusicByIdAsync(GetMusicByIdRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyCollection<MusicDto>> GetMusicsByArtistAsync(GetMusicsByArtistRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task AddMusicToAlbumAsync(AddMusicToAlbumRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteMusicAsync(DeleteMusicRequest request)
    {
        throw new NotImplementedException();
    }
}
