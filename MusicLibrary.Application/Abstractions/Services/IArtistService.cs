using MusicLibrary.Application.Requests.Artist;
using MusicLibrary.Application.Responses.DTOs;

namespace MusicLibrary.Application.Abstractions.Services;

public interface IArtistService
{
    Task<ArtistDto> CreateAsync(CreateArtistRequest request);
    Task<ArtistDto?> GetByIdAsync(GetArtistByIdRequest request);
    Task<IReadOnlyCollection<ArtistDto>> GetAllAsync();
    Task<ArtistDto> UpdateAsync(UpdateArtistRequest request);
    Task DeleteAsync(DeleteArtistRequest request);
}