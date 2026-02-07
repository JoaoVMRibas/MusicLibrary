using MusicLibrary.Domain.Exceptions;

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

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("The artist's name cannot be null or empty.", nameof(name));

        if (Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            throw new ArtistAlreadyHasThisNameException(name);

        Name = name; 
    }

    public Album AddAlbum(string name)
    {
        if (_albums.Any(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            throw new DuplicateAlbumException(name);

        var album = new Album(this.Id, name);
        _albums.Add(album);
        return album;
    }

    public void RemoveAlbum(Guid albumId)
    {
        var album = _albums.FirstOrDefault(a => a.Id == albumId) ?? throw new AlbumNotFoundException();

        _albums.Remove(album);
    }

    public Music AddMusic(string name, TimeSpan duration) 
    {
        if (_musics.Any(m => m.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            throw new DuplicateMusicException(name);

        var music = new Music(this.Id,name, duration);
        _musics.Add(music);
        return music;
    }

    public void RemoveMusic(Guid musicId)
    {
        var music = _musics.FirstOrDefault(m => m.Id == musicId) ?? throw new MusicNotFoundException();

        _musics.Remove(music);
    }

    public void AddMusicToAlbum(Guid albumId, Guid musicId)
    {
        var album = _albums.FirstOrDefault(a => a.Id == albumId) ?? throw new AlbumNotFoundException();
        var music = _musics.FirstOrDefault(m => m.Id == musicId) ?? throw new MusicNotFoundException();

        album.AddMusic(music);
    }
}
