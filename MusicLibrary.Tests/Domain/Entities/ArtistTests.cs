using MusicLibrary.Domain.Entities;
using MusicLibrary.Domain.Exceptions;
using System.Threading.Tasks;

namespace MusicLibrary.Tests.Domain.Entities;

public class ArtistTests
{
    [Fact]
    public void Should_Create_Artist_With_Valid_Name()
    {
        //Arrange
        var name = "Metallica";

        //Act
        var artist = new Artist(name);

        //Assert
        Assert.NotEqual(Guid.Empty, artist.Id);
        Assert.Equal(name, artist.Name);
    }

    [Fact]
    public void Should_Throw_Exception_When_Name_Is_Null_Or_Empty()
    {
        //Arrange
        var name = string.Empty;

        //Act and Assert
        var exception = Assert.Throws<ArgumentException>(() => new Artist(name));

        //Assert
        Assert.Equal("name", exception.ParamName);
        Assert.Contains("cannot be null or empty.", exception.Message);
    }

    [Fact]
    public void Should_Update_Artist_Name_With_New_Valid_Name()
    {
        //Arrange
        var name = "Metallica";

        //Act
        var artist = new Artist(name);
        artist.UpdateName("Guns");

        //Assert
        Assert.NotEqual(Guid.Empty, artist.Id);
        Assert.Equal("Guns", artist.Name);
    }

    [Fact]
    public void Should_Throw_Exception_When_Updated_New_Name_Is_Null_Or_Empty()
    {
        //Arrange
        var name = "Metallica";
        var artist = new Artist(name);

        //Act and Assert
        var exception = Assert.Throws<ArgumentException>(() => artist.UpdateName(string.Empty));

        //Assert
        Assert.Equal("name", exception.ParamName);
        Assert.Contains("cannot be null or empty.", exception.Message);
    }

    [Fact]
    public void Should_Throw_Exception_When_Updated_New_Name_Is_Equal_To_Actual_Name()
    {
        //Arrange
        var name = "Metallica";
        var artist = new Artist(name);

        //Act and Assert
        var exception = Assert.Throws<ArtistAlreadyHasThisNameException>(() => artist.UpdateName("Metallica"));

        //Assert
        Assert.Contains($"The artist already has the name '{artist.Name}'.", exception.Message);
    }

    [Fact]
    public void Should_Add_Music_To_Artist_When_Data_Is_Valid()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var musicName = "Nothing Else Matters";
        TimeSpan duration = TimeSpan.FromSeconds(386);

        //Act
        var music = artist.AddMusic(musicName, duration);

        //Assert
        Assert.Single(artist.Musics);
        Assert.NotEqual(Guid.Empty, music.Id);
        Assert.Equal(musicName, music.Name);
        Assert.Equal(duration, music.Duration);
        Assert.Equal(artist.Id, music.ArtistId);
    }

    [Fact]
    public void Should_Throw_Exception_When_Adding_Music_With_Invalid_Name()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var musicName = string.Empty;
        TimeSpan duration = TimeSpan.FromSeconds(386);

        //Act
        var exception = Assert.Throws<ArgumentException>(() => artist.AddMusic(musicName, duration));

        //Assert
        Assert.Empty(artist.Musics);
        Assert.Equal("name", exception.ParamName);
        Assert.Contains("cannot be null or empty.", exception.Message);
    }

    [Fact]
    public void Should_Throw_Exception_When_Adding_Music_With_Invalid_Duration()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var musicName = "Nothing Else Matters";
        TimeSpan duration = TimeSpan.FromSeconds(0);

        //Act
        var exception = Assert.Throws<ArgumentException>(() => artist.AddMusic(musicName, duration));

        //Assert
        Assert.Empty(artist.Musics);
        Assert.Equal("duration", exception.ParamName);
        Assert.Contains("cannot be equal to or less than zero.", exception.Message);
    }

    [Fact]
    public void Should_Remove_Music_From_Artist_When_Music_Is_Found()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var music = artist.AddMusic("Fade to Black", TimeSpan.FromSeconds(415));

        //Act and Assert
        Assert.Single(artist.Musics);
        artist.RemoveMusic(music.Id);
        Assert.Empty(artist.Musics);
    }

    [Fact]
    public void Should_Throw_Exception_When_Removing_Nonexistent_Music_From_Artist()
    {
        //Arrange
        var artist = new Artist("Metallica");

        //Act and Assert
        var exception = Assert.Throws<MusicNotFoundException>(() => artist.RemoveMusic(Guid.NewGuid()));

        //Assert
        Assert.Empty(artist.Musics);
        Assert.Contains("Music not found.", exception.Message);
    }

    [Fact]
    public async Task Should_Throw_Exception_When_Removing_Music_In_Album_From_Artist()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var album = artist.AddAlbum("The Black Album");
        var music = artist.AddMusic("Enter Sandman", TimeSpan.FromSeconds(331));
        artist.AddMusicToAlbum(album.Id, music.Id);

        //Act
        var exception = Assert.Throws<MusicInAlbumException>(() => artist.RemoveMusic(music.Id));

        //Assert
        Assert.Single(artist.Musics);
        Assert.Contains("The music is part of an album and cannot be removed.", exception.Message);
    }

    [Fact]
    public void Should_Add_Album_To_Artist_When_Data_Is_Valid()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var albumName = "Master of Puppets";

        //Act
        var album = artist.AddAlbum(albumName);

        //Assert
        Assert.Single(artist.Albums);
        Assert.NotEqual(Guid.Empty, album.Id);
        Assert.Equal(albumName, album.Name);
        Assert.Equal(artist.Id, album.ArtistId);
    }

    [Fact]
    public void Should_Throw_Exception_When_Adding_Album_With_Invalid_Name()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var albumName = string.Empty;

        //Act
        var exception = Assert.Throws<ArgumentException>(() => artist.AddAlbum(albumName));

        //Assert
        Assert.Empty(artist.Albums);
        Assert.Equal("name", exception.ParamName);
        Assert.Contains("cannot be null or empty.", exception.Message);
    }

    [Fact]
    public void Should_Remove_Album_From_Artist_When_Album_Is_Found()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var album = artist.AddAlbum("Master of Puppets");

        //Act and Assert
        Assert.Single(artist.Albums);
        artist.RemoveAlbum(album.Id);
        Assert.Empty(artist.Albums);
    }

    [Fact]
    public void Should_Throw_Exception_When_Removing_Nonexistent_Album_From_Artist()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var album = artist.AddAlbum("Master of Puppets");

        //Act and Assert
        var exception = Assert.Throws<AlbumNotFoundException>(()=> artist.RemoveAlbum(Guid.NewGuid()));

        //Assert
        Assert.Single(artist.Albums);
        Assert.Contains("Album not found.", exception.Message);
    }

    [Fact]
    public void Should_Add_Music_To_Album()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var album = artist.AddAlbum("The Black Album");
        var music = artist.AddMusic("Enter Sandman", TimeSpan.FromSeconds(331));

        //Act
        artist.AddMusicToAlbum(album.Id, music.Id);

        //Assert
        Assert.Single(artist.Albums);
        Assert.Single(album.Musics);
        Assert.Contains(album.Musics, m => m.Id == music.Id);
    }

    [Fact]
    public void Should_Throw_Exception_When_Adding_Nonexistent_Music_To_Album()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var album = artist.AddAlbum("The Black Album");
        var music = artist.AddMusic("Enter Sandman", TimeSpan.FromSeconds(331));

        //Act
        var exception = Assert.Throws<MusicNotFoundException>(() => artist.AddMusicToAlbum(album.Id, Guid.NewGuid()));

        //Assert
        Assert.Empty(album.Musics);
        Assert.Contains("Music not found.", exception.Message);
    }

    [Fact]
    public void Should_Throw_Exception_When_Adding_Music_To_Nonexistent_Album()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var album = artist.AddAlbum("The Black Album");
        var music = artist.AddMusic("Enter Sandman", TimeSpan.FromSeconds(331));

        //Act
        var exception = Assert.Throws<AlbumNotFoundException>(() => artist.AddMusicToAlbum(Guid.NewGuid(), music.Id));

        //Assert
        Assert.Contains("Album not found.", exception.Message);
    }

    [Fact]
    public void Should_Not_Allow_Same_Music_Twice_In_Album()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var album = artist.AddAlbum("The Black Album");
        var music = artist.AddMusic("Enter Sandman", TimeSpan.FromSeconds(331));

        //Act
        artist.AddMusicToAlbum(album.Id, music.Id);
        var exception = Assert.Throws<MusicAlreadyInAlbumException>(() => artist.AddMusicToAlbum(album.Id, music.Id));

        //Assert
        Assert.Single(album.Musics);
        Assert.Contains("Music already added to album.", exception.Message);
    }

    [Fact]
    public void Should_Remove_Music_From_Album()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var album = artist.AddAlbum("The Black Album");
        var music = artist.AddMusic("Enter Sandman", TimeSpan.FromSeconds(331));
        artist.AddMusicToAlbum(album.Id, music.Id);

        //Act
        artist.RemoveMusicFromAlbum(album.Id, music.Id);

        //Assert
        Assert.Empty(album.Musics);
    }

    [Fact]
    public async Task Should_Throw_Exception_When_Removing_Music_Not_In_Album()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var album = artist.AddAlbum("The Black Album");
        var music = artist.AddMusic("Enter Sandman", TimeSpan.FromSeconds(331));
        artist.AddMusicToAlbum(album.Id, music.Id);

        //Act
        var exception = Assert.Throws<MusicNotFoundException>(() => artist.RemoveMusicFromAlbum(album.Id, Guid.NewGuid()));

        //Assert
        Assert.Single(album.Musics);
        Assert.Contains("Music not found.",exception.Message);
    }

    [Fact]
    public async Task Should_Throw_Exception_When_Removing_Music_To_Nonexistent_Album()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var album = artist.AddAlbum("The Black Album");
        var music = artist.AddMusic("Enter Sandman", TimeSpan.FromSeconds(331));
        artist.AddMusicToAlbum(album.Id, music.Id);

        //Act
        var exception = Assert.Throws<AlbumNotFoundException>(() => artist.RemoveMusicFromAlbum(Guid.NewGuid(),music.Id));

        //Assert
        Assert.Single(album.Musics);
        Assert.Contains("Album not found.", exception.Message);
    }

    [Fact]
    public void Album_Duration_Should_Be_Sum_Of_All_Music_Durations()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var album = artist.AddAlbum("The Black Album");
        var music = artist.AddMusic("Enter Sandman", TimeSpan.FromSeconds(331));
        var music2 = artist.AddMusic("The Unforgiven", TimeSpan.FromSeconds(387));

        //Act
        artist.AddMusicToAlbum(album.Id, music.Id);
        artist.AddMusicToAlbum(album.Id, music2.Id);

        //Assert
        Assert.Equal(TimeSpan.FromSeconds(718), album.Duration);
    }

    [Fact]
    public void Should_Not_Allow_Creating_Album_With_Duplicated_Name()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var album = artist.AddAlbum("The Black Album");

        //Act
        var exception = Assert.Throws<DuplicateAlbumException>(() => artist.AddAlbum("The Black Album"));

        //Assert
        Assert.Single(artist.Albums);
        Assert.Contains($"Album '{album.Name}' already exists.", exception.Message);
    }

    [Fact]
    public void Should_Not_Allow_Creating_Music_With_Duplicated_Name()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var album = artist.AddAlbum("The Black Album");
        var music = artist.AddMusic("Enter Sandman", TimeSpan.FromSeconds(331));

        //Act
        var exception = Assert.Throws<DuplicateMusicException>(() => artist.AddMusic("Enter Sandman", TimeSpan.FromSeconds(331)));

        //Assert
        Assert.Single(artist.Musics);
        Assert.Contains($"Music '{music.Name}' already exists.", exception.Message);
    }
}