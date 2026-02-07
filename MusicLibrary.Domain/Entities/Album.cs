using MusicLibrary.Domain.Exceptions;

namespace MusicLibrary.Domain.Entities;

public class Album
{
    public Guid Id { get; private set; }
    public Guid ArtistId { get; private set; }
    public string Name { get; private set; }
    public TimeSpan Duration => _albumMusics.Aggregate(TimeSpan.Zero, (total, music) => total + music.Duration);

    private readonly List<Music> _albumMusics = [];
    public IReadOnlyCollection<Music> Musics => _albumMusics;

    internal Album(Guid artistId,string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("The album name cannot be null or empty.", nameof(name));

        Id = Guid.NewGuid();
        Name = name;
        ArtistId = artistId;
    }
    
    internal void AddMusic(Music music)
    {
        ArgumentNullException.ThrowIfNull(music);

        if (_albumMusics.Any(m => m.Id == music.Id))
            throw new MusicAlreadyInAlbumException();

        _albumMusics.Add(music);
    }

    internal Music GetMusicById(Guid musicId)
    {
        var music = _albumMusics.FirstOrDefault(m => m.Id == musicId) ?? throw new MusicNotFoundException();
        return music;
    }

    internal void RemoveMusic(Guid musicId)
    {
        var music = GetMusicById(musicId);
        _albumMusics.Remove(music);
    }
}