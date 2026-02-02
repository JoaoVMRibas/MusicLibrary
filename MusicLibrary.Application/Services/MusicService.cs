using MusicLibrary.Application.Abstractions.Services;
using MusicLibrary.Application.Requests.Music;
using MusicLibrary.Application.Responses.DTOs;
using MusicLibrary.Application.Abstractions.Repositories;

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
        var artist = await _artistRepository.GetByIdAsync(request.ArtistId) ?? throw new InvalidOperationException("Artist not found.");

        var music = artist.AddMusic(request.Name, request.Duration);

        await _artistRepository.UpdateAsync(artist);

        return new MusicDto(music.Id,music.Name,music.Duration);
    }

    public async Task<MusicDto?> GetMusicByIdAsync(GetMusicByIdRequest request)
    {
        var artist = await _artistRepository.GetByIdAsync(request.ArtistId) ?? throw new InvalidOperationException("Artist not found.");

        var music = artist.Musics.FirstOrDefault(m =>  m.Id == request.MusicId) ?? throw new InvalidOperationException("Music not found.");

        return new MusicDto(music.Id, music.Name, music.Duration);
    }

    public async Task<IReadOnlyCollection<MusicDto>> GetMusicsByArtistAsync(GetMusicsByArtistRequest request)
    {
        var artist = await _artistRepository.GetByIdAsync(request.ArtistId) ?? throw new InvalidOperationException("Artist not found.");

        return artist.Musics.Select(m => new MusicDto(m.Id, m.Name, m.Duration)).ToList();
    }

    public async Task AddMusicToAlbumAsync(AddMusicToAlbumRequest request)
    {
        var artist = await _artistRepository.GetByIdAsync(request.ArtistId) ?? throw new InvalidOperationException("Artist not found.");
        var album = artist.Albums.FirstOrDefault(a => a.Id == request.AlbumId) ?? throw new InvalidOperationException("Album not found.");
        var music = artist.Musics.FirstOrDefault(m => m.Id == request.MusicId) ?? throw new InvalidOperationException("Music not found.");

        artist.AddMusicToAlbum(album.Id, music.Id);

        await _artistRepository.UpdateAsync(artist);
    }

    public async Task DeleteMusicAsync(DeleteMusicRequest request)
    {
        var artist = await _artistRepository.GetByIdAsync(request.ArtistId) ?? throw new InvalidOperationException("Artist not found.");

        artist.RemoveMusic(request.MusicId);

        await _artistRepository.UpdateAsync(artist);
    }
}
