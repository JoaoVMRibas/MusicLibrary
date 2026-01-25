using MusicLibrary.Application.Requests;
using MusicLibrary.Application.Responses.DTOs;

namespace MusicLibrary.Application.Abstractions.Services;

public interface IArtistService
{
    Task<ArtistDto> CreateAsync(CreateArtistRequest request);
    Task<ArtistDto?> GetByIdAsync(Guid id);
    Task<IReadOnlyCollection<ArtistDto>> GetAllAsync();
    Task<ArtistDto> UpdateAsync(UpdateArtistRequest request);
    Task DeleteAsync(Guid Id);
}