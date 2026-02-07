using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicLibrary.Application.Abstractions.Repositories;
using MusicLibrary.Domain.Entities;
using MusicLibrary.Infrastructure.Data;

namespace MusicLibrary.Infrastructure.Repositories;

public class ArtistRepository : IArtistRepository
{
    private readonly MusicLibraryDbContext _context;
    public ArtistRepository(MusicLibraryDbContext context)
    {
        _context = context;
    }

    public async Task<Artist?> GetByIdAsync(Guid id)
    {
        return await _context.Artists
                        .Include(a => a.Albums)
                            .ThenInclude(al => al.Musics)
                        .Include(a => a.Musics)
                        .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IReadOnlyCollection<Artist>> GetAllAsync()
    {
        return await _context.Artists
                        .AsNoTracking()
                        .ToListAsync();
    }

    public async Task AddAsync(Artist artist)
    {
        await _context.Artists.AddAsync(artist);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Artist artist)
    {
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Artist artist)
    {
        _context.Artists.Remove(artist);
        await _context.SaveChangesAsync();
    }
}
