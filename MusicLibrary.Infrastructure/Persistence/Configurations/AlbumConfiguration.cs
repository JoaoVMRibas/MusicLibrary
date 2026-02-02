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

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasMany<Music>(a=>a.Musics)
               .WithOne()                       
               .HasForeignKey("AlbumId")        
               .IsRequired(false)
               .OnDelete(DeleteBehavior.SetNull);

        builder.Navigation(a => a.Musics)
               .UsePropertyAccessMode(PropertyAccessMode.Field)
               .Metadata.SetField("_albumMusics");
    }
}