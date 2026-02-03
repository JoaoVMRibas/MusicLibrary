using MusicLibrary.Application.Requests.Artist;
using MusicLibrary.Application.Responses.DTOs;

namespace MusicLibrary.Application.Abstractions.Services;

public interface IArtistService
{
    Task<ArtistDto> CreateAsync(CreateArtistRequest request);
    Task<ArtistDto?> GetByIdAsync(Guid id);
    Task<IReadOnlyCollection<ArtistDto>> GetAllAsync();
    Task<ArtistDto> UpdateAsync(Guid id,UpdateArtistRequest request);
    Task DeleteAsync(Guid id);
}