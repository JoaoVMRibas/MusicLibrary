using MusicLibrary.Application.Abstractions.Repositories;
using MusicLibrary.Application.Abstractions.Services;
using MusicLibrary.Application.Requests.Artist;
using MusicLibrary.Application.Responses.DTOs;
using MusicLibrary.Domain.Entities;

namespace MusicLibrary.Application.Services;

public class ArtistService : IArtistService
{
    private readonly IArtistRepository _artistRepository;
    public ArtistService(IArtistRepository artistRepository)
    {
        _artistRepository = artistRepository;
    }

    public async Task<ArtistDto> CreateAsync(CreateArtistRequest request)
    {
        var artist = new Artist(request.Name);

        await _artistRepository.AddAsync(artist);

        return new ArtistDto(artist.Id, artist.Name);
    }

    public async Task<IReadOnlyCollection<ArtistDto>> GetAllAsync()
    {
        var artistList = await _artistRepository.GetAllAsync();

        return artistList.Select(artist => new ArtistDto(artist.Id,artist.Name)).ToList();
    }

    public async Task<ArtistDto?> GetByIdAsync(GetArtistByIdRequest request)
    {
        var artist = await _artistRepository.GetByIdAsync(request.ArtistId);

        if(artist == null) throw new InvalidOperationException("Artist not found.");

        return new ArtistDto(artist.Id,artist.Name);
    }

    public async Task<ArtistDto> UpdateAsync(UpdateArtistRequest request)
    {
        var artist = await _artistRepository.GetByIdAsync(request.ArtistId);

        if (artist == null) throw new InvalidOperationException("Artist not found.");

        artist.UpdateName(request.Name);

        await _artistRepository.UpdateAsync(artist);

        return new ArtistDto(artist.Id, artist.Name);
    }

    public async Task DeleteAsync(DeleteArtistRequest request)
    {
        var artist = await _artistRepository.GetByIdAsync(request.ArtistId);

        if( artist == null) throw new InvalidOperationException("Artist not found.");

        await _artistRepository.DeleteAsync(artist);
    }

}
