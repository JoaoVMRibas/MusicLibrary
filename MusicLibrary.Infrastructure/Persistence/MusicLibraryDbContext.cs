using Microsoft.EntityFrameworkCore;
using MusicLibrary.Domain.Entities;

namespace MusicLibrary.Infrastructure.Persistence;

public class MusicLibraryDbContext : DbContext
{
    public MusicLibraryDbContext(DbContextOptions<MusicLibraryDbContext> options) 
        : base(options) { }

    public DbSet<Artist> Artists => Set<Artist>();
    public DbSet<Album> Albums => Set<Album>();
    public DbSet<Music> Musics => Set<Music>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MusicLibraryDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
