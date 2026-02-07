using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicLibrary.Domain.Entities;

namespace MusicLibrary.Infrastructure.Data.Configurations;

public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        builder.ToTable("Artists");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
               .ValueGeneratedNever();

        builder.Property(a => a.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.HasMany(a => a.Albums)
               .WithOne()
               .HasForeignKey(al => al.ArtistId) 
               .OnDelete(DeleteBehavior.Cascade);


        builder.Navigation(a => a.Albums)
               .UsePropertyAccessMode(PropertyAccessMode.Field)
               .Metadata.SetField("_albums");

        builder.HasMany(a => a.Musics)
               .WithOne()
               .HasForeignKey(m => m.ArtistId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(a => a.Musics)
               .UsePropertyAccessMode(PropertyAccessMode.Field)
               .Metadata.SetField("_musics");
    }
}