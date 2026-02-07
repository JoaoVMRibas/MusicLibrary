using Microsoft.EntityFrameworkCore;
using MusicLibrary.Domain.Entities;
using System.Reflection;

namespace MusicLibrary.Infrastructure.Data;

public class MusicLibraryDbContext : DbContext
{
    public MusicLibraryDbContext(DbContextOptions<MusicLibraryDbContext> options) : base(options) { }

    public DbSet<Artist> Artists { get; set; }
    public DbSet<Album> Albums { get; set; }
    public DbSet<Music> Musics { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}