namespace MusicLibrary.Domain.Entities;

public class Music
{
    public Guid Id { get; private set; }
    public Guid ArtistId { get; private set; }
    public string Name { get; private set; }
    public TimeSpan Duration { get; private set; }

    internal Music(Guid artistId, string name, TimeSpan duration)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("The music name cannot be null or empty.", nameof(name));

        if(duration <= TimeSpan.Zero)
            throw new ArgumentException("The duration of the music cannot be equal to or less than zero.", nameof(duration));

        Id = Guid.NewGuid();
        Name = name;
        Duration = duration;
        ArtistId = artistId;
    }
}