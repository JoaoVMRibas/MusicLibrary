using MusicLibrary.Domain.Entities;

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
        var name = "";

        //Act and Assert
        var exception = Assert.Throws<ArgumentException>(() => new Artist(name));

        //Assert
        Assert.Equal("name", exception.ParamName);
        Assert.Contains("cannot be null or empty.", exception.Message);
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
    }

    [Fact]
    public void Should_Throw_Exception_When_Adding_Music_With_Invalid_Name()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var musicName = "";
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
    }

    [Fact]
    public void Should_Throw_Exception_When_Adding_Album_With_Invalid_Name()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var albumName = "";

        //Act
        var exception = Assert.Throws<ArgumentException>(() => artist.AddAlbum(albumName));

        //Assert
        Assert.Empty(artist.Albums);
        Assert.Equal("name", exception.ParamName);
        Assert.Contains("cannot be null or empty.", exception.Message);
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
        var exception = Assert.Throws<InvalidOperationException>(() => artist.AddMusicToAlbum(album.Id, Guid.NewGuid()));

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
        var exception = Assert.Throws<InvalidOperationException>(() => artist.AddMusicToAlbum(Guid.NewGuid(), music.Id));

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
        var exception = Assert.Throws<InvalidOperationException>(() => artist.AddMusicToAlbum(album.Id, music.Id));

        //Assert
        Assert.Single(album.Musics);
        Assert.Contains("Music already added to album.", exception.Message);
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
        var exception = Assert.Throws<InvalidOperationException>(() => artist.AddAlbum("The Black Album"));

        //Assert
        Assert.Single(artist.Albums);
        Assert.Contains("Album with the same name already exists.", exception.Message);
    }

    [Fact]
    public void Should_Not_Allow_Creating_Music_With_Duplicated_Name()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var album = artist.AddAlbum("The Black Album");
        var music = artist.AddMusic("Enter Sandman", TimeSpan.FromSeconds(331));

        //Act
        var exception = Assert.Throws<InvalidOperationException>(() => artist.AddMusic("Enter Sandman", TimeSpan.FromSeconds(331)));

        //Assert
        Assert.Single(artist.Musics);
        Assert.Contains("Music with the same name already exists.", exception.Message);
    }
}