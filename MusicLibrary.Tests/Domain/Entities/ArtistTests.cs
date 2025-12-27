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
        Assert.NotNull(artist);
        Assert.Equal(name, artist.Name);
        Assert.NotEqual(Guid.Empty, artist.Id);
    }

    [Fact]
    public void Should_Throw_Exception_When_Name_Is_Null_Or_Empty()
    {
        //Arrange
        var name = "";

        //Act and Assert
        var exception = Assert.Throws<ArgumentException>(() => new Artist(name));

        //Assert
        Assert.Equal("name",exception.ParamName);
        Assert.Contains("cannot be null or empty.", exception.Message);
    }

    [Fact]
    public void Should_Create_Music_With_Valid_Name()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var musicName = "Nothing Else Matters";

        //Act
        artist.AddMusic(musicName);
        var music = artist.Musics.First();

        //Assert
        Assert.Single(artist.Musics);
        Assert.NotEqual(Guid.Empty,music.Id);
        Assert.Equal(musicName, music.Name);
    }

    [Fact]
    public void Should_Throw_Exception_When_Adding_Music_With_Invalid_Name()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var musicName = "";

        //Act
        var exception = Assert.Throws<ArgumentException>(() => artist.AddMusic(musicName));

        //Assert
        Assert.Empty(artist.Musics);
        Assert.Equal("name", exception.ParamName);
        Assert.Contains("cannot be null or empty.", exception.Message);
    }

    [Fact]
    public void Should_Create_Album_With_Valid_Name()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var albumName = "Master of Puppets";

        //Act
        artist.AddAlbum(albumName);
        var album = artist.Albums.First();

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
}
