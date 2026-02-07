using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicLibrary.Domain.Entities;

namespace MusicLibrary.Infrastructure.Persistence.Configurations;

public class AlbumConfiguration : IEntityTypeConfiguration<Album>
{
    public void Configure(EntityTypeBuilder<Album> builder)
    {
        builder.ToTable("Albums");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
               .ValueGeneratedNever();

        builder.Property(a => a.Name).IsRequired().HasMaxLength(200);

        builder.Property(a => a.ArtistId).IsRequired();

        builder.Ignore(a => a.Duration);

        builder.HasMany(a => a.Musics)
               .WithMany()
               .UsingEntity<Dictionary<string, object>>(
                   "AlbumMusics",
                   j => j.HasOne<Music>().WithMany().HasForeignKey("MusicId").OnDelete(DeleteBehavior.Restrict),
                   j => j.HasOne<Album>().WithMany().HasForeignKey("AlbumId").OnDelete(DeleteBehavior.Cascade)
               );

        builder.Navigation(a => a.Musics)
               .UsePropertyAccessMode(PropertyAccessMode.Field)
               .Metadata.SetField("_albumMusics");
    }
}