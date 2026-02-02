using MusicLibrary.Application.Abstractions.Repositories;
using MusicLibrary.Application.Abstractions.Services;
using MusicLibrary.Application.Exceptions;
using MusicLibrary.Application.Requests.Album;
using MusicLibrary.Application.Responses.DTOs;
using MusicLibrary.Domain.Exceptions;

namespace MusicLibrary.Application.Services;

public class AlbumService : IAlbumService
{
    private readonly IArtistRepository _artistRepository;

    public AlbumService(IArtistRepository artistRepository)
    {
        _artistRepository = artistRepository;
    }

    public async Task<AlbumDto> CreateAlbumAsync(CreateAlbumRequest request)
    {
        var artist = await _artistRepository.GetByIdAsync(request.ArtistId) ?? throw new ArtistNotFoundException();

        var album = artist.AddAlbum(request.Name);

        await _artistRepository.UpdateAsync(artist);

        return new AlbumDto(album.Id, album.Name,album.Duration);
    }

    public async Task<AlbumDto?> GetAlbumByIdAsync(GetAlbumByIdRequest request)
    {
        var artist = await _artistRepository.GetByIdAsync(request.ArtistId) ?? throw new ArtistNotFoundException();

        var album = artist.Albums.FirstOrDefault(a => a.Id == request.AlbumId) ?? throw new AlbumNotFoundException();

        return new AlbumDto(album.Id,album.Name,album.Duration);
    }

    public async Task<IReadOnlyCollection<AlbumDto>> GetAlbumsByArtist(GetAlbumsByArtistRequest request)
    {
        var artist = await _artistRepository.GetByIdAsync(request.ArtistId) ?? throw new ArtistNotFoundException();

        return artist.Albums.Select(a => new AlbumDto(a.Id,a.Name,a.Duration)).ToList();
    }

    public async Task DeleteAlbumAsync(DeleteAlbumRequest request)
    {
        var artist = await _artistRepository.GetByIdAsync(request.ArtistId) ?? throw new ArtistNotFoundException();

        artist.RemoveAlbum(request.AlbumId);

        await _artistRepository.UpdateAsync(artist);
    }
}
