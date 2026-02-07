using MusicLibrary.Domain.Entities;

namespace MusicLibrary.Application.Abstractions.Repositories;

public interface IArtistRepository
{
    Task<Artist?> GetByIdAsync(Guid id);
    Task<IReadOnlyCollection<Artist>> GetAllAsync();
    Task AddAsync(Artist artist);
    Task UpdateAsync(Artist artist);
    Task DeleteAsync(Artist artist);
}