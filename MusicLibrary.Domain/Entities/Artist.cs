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

    private Album GetAlbumById(Guid albumId)
    {
        return _albums.FirstOrDefault(a => a.Id == albumId) ?? throw new AlbumNotFoundException();
    }   

    public void RemoveAlbum(Guid albumId)
    {
        var album = GetAlbumById(albumId);

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

    private Music GetMusicById(Guid musicId)
    {
        return _musics.FirstOrDefault(m => m.Id == musicId) ?? throw new MusicNotFoundException();
    }

    public void RemoveMusic(Guid musicId)
    {
        if(IsMusicInAnyAlbum(musicId)) 
            throw new MusicInAlbumException();
        
        var music = GetMusicById(musicId);

        _musics.Remove(music);
    }

    private bool IsMusicInAnyAlbum(Guid musicId)
    {
        return _albums.Any(a => a.Musics.Any(m => m.Id == musicId));
    }

    public void AddMusicToAlbum(Guid albumId, Guid musicId)
    {
        var album = GetAlbumById(albumId);
        var music = GetMusicById(musicId);

        album.AddMusic(music);
    }

    public void RemoveMusicFromAlbum(Guid albumId, Guid musicId)
    {
        var album = GetAlbumById(albumId);

        album.RemoveMusic(musicId);
    }
}