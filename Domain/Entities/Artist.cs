namespace MusicLibrary.Domain.Entities;

public class Artist
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    private readonly List<Album> _albums  = [];
    private readonly List<Music> _musics  = [];

    public IReadOnlyCollection<Album> Albums => _albums;
    public IReadOnlyCollection<Music> Musics => _musics;

    public Artist(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("The artist's name cannot be null or empty.", nameof(name));

        Id = Guid.NewGuid();
        Name = name;
    }

    public void AddAlbum(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("The album name cannot be null or empty.", nameof(name));

        _albums.Add(new Album(name));
    }
    public void AddMusic(string name) 
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("The music name cannot be null or empty.", nameof(name));

        _musics.Add(new Music(name));
    }
}
