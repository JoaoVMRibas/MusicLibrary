using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicLibrary.Domain.Entities;

namespace MusicLibrary.Infrastructure.Persistence.Configurations;

public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        builder.ToTable("Artists");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.HasMany<Album>(a => a.Albums)
               .WithOne()
               .HasForeignKey("ArtistId")
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany<Music>(a => a.Musics)
               .WithOne()
               .HasForeignKey("ArtistId")
               .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(a => a.Albums)
               .UsePropertyAccessMode(PropertyAccessMode.Field)
               .Metadata.SetField("_albums");

        builder.Navigation(a => a.Musics)
               .UsePropertyAccessMode(PropertyAccessMode.Field)
               .Metadata.SetField("_musics");
    }
}