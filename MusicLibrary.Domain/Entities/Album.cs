using MusicLibrary.Domain.Exceptions;

namespace MusicLibrary.Domain.Entities;

public class Album
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    private readonly List<Music> _musics = [];
    public IReadOnlyCollection<Music> Musics => _musics;
    public TimeSpan Duration => _musics.Aggregate(TimeSpan.Zero, (total, music) => total + music.Duration);
    internal Album(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("The album name cannot be null or empty.", nameof(name));

        Id = Guid.NewGuid();
        Name = name;
    }
    internal void AddMusic(Music music)
    {
        ArgumentNullException.ThrowIfNull(music);

        if (_musics.Any(m => m.Id == music.Id))
            throw new MusicAlreadyInAlbumException();

        _musics.Add(music);
    }
}
