using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicLibrary.Domain.Entities;

namespace MusicLibrary.Infrastructure.Persistence.Configurations;

public class MusicConfiguration : IEntityTypeConfiguration<Music>
{
    public void Configure(EntityTypeBuilder<Music> builder)
    {
        builder.ToTable("Musics");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
               .ValueGeneratedNever();

        builder.Property(m => m.ArtistId)
            .IsRequired();

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.Duration)
            .IsRequired();
    }
}
