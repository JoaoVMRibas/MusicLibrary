using MusicLibrary.Application.Abstractions.Repositories;
using MusicLibrary.Application.Abstractions.Services;
using MusicLibrary.Application.Exceptions;
using MusicLibrary.Application.Requests.Music;
using MusicLibrary.Application.Responses.DTOs;
using MusicLibrary.Domain.Exceptions;

namespace MusicLibrary.Application.Services;

public class MusicService : IMusicService
{
    private readonly IArtistRepository _artistRepository;

    public MusicService(IArtistRepository artistRepository)
    {
        _artistRepository = artistRepository;
    }

    public async Task<MusicDto> CreateMusicAsync(CreateMusicRequest request)
    {
        var artist = await _artistRepository.GetByIdAsync(request.ArtistId) ?? throw new ArtistNotFoundException();

        var music = artist.AddMusic(request.Name, request.Duration);

        await _artistRepository.UpdateAsync(artist);

        return new MusicDto(music.Id,music.Name,music.Duration);
    }

    public async Task<MusicDto?> GetMusicByIdAsync(GetMusicByIdRequest request)
    {
        var artist = await _artistRepository.GetByIdAsync(request.ArtistId) ?? throw new ArtistNotFoundException();

        var music = artist.Musics.FirstOrDefault(m => m.Id == request.MusicId) ?? throw new MusicNotFoundException();

        return new MusicDto(music.Id, music.Name, music.Duration);
    }

    public async Task<IReadOnlyCollection<MusicDto>> GetMusicsByArtistAsync(GetMusicsByArtistRequest request)
    {
        var artist = await _artistRepository.GetByIdAsync(request.ArtistId) ?? throw new ArtistNotFoundException();

        return artist.Musics.Select(m => new MusicDto(m.Id, m.Name, m.Duration)).ToList();
    }

    public async Task AddMusicToAlbumAsync(AddMusicToAlbumRequest request)
    {
        var artist = await _artistRepository.GetByIdAsync(request.ArtistId) ?? throw new ArtistNotFoundException();

        artist.AddMusicToAlbum(request.AlbumId, request.MusicId);

        await _artistRepository.UpdateAsync(artist);
    }

    public async Task DeleteMusicAsync(DeleteMusicRequest request)
    {
        var artist = await _artistRepository.GetByIdAsync(request.ArtistId) ?? throw new ArtistNotFoundException();

        artist.RemoveMusic(request.MusicId);

        await _artistRepository.UpdateAsync(artist);
    }
}
