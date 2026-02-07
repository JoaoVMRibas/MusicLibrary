using MusicLibrary.Application.Abstractions.Repositories;
using MusicLibrary.Application.Abstractions.Services;
using MusicLibrary.Application.Exceptions;
using MusicLibrary.Application.Requests.Album;
using MusicLibrary.Application.Services;
using MusicLibrary.Domain.Entities;
using MusicLibrary.Domain.Exceptions;
using NSubstitute;

namespace MusicLibrary.Tests.Application.Services;

public class AlbumServiceTests
{
    private readonly IArtistRepository _artistRepository;
    private readonly IAlbumService _albumService;

    public AlbumServiceTests()
    {
        _artistRepository = Substitute.For<IArtistRepository>();
        _albumService = new AlbumService(_artistRepository);
    }

    [Fact]
    public async Task CreateAlbumAsync_Should_Add_Album_To_Artist()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var request = new CreateAlbumRequest(artist.Id,"Ride the Lightning");

        _artistRepository.GetByIdAsync(artist.Id).Returns(artist);

        //Act
        var response = await _albumService.CreateAlbumAsync(request);

        //Assert
        Assert.Single(artist.Albums);
        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.Equal("Ride the Lightning", response.Name);
        Assert.Equal(TimeSpan.Zero, response.Duration);

        await _artistRepository.Received(1).UpdateAsync(artist);
    }

    [Fact]
    public async Task CreateAlbumAsync_Should_Throw_When_Artist_Not_Found()
    {
        //Arange
        var artist = new Artist("Metallica");

        _artistRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((Artist?)null);

        //Act and Assert
        var exception = await Assert.ThrowsAsync<ArtistNotFoundException>(
            () => _albumService.CreateAlbumAsync(new CreateAlbumRequest(artist.Id,"Ride the Lightning"))
        );

        //Assert
        Assert.Contains("Artist not found.", exception.Message);
        Assert.Empty(artist.Albums);

        await _artistRepository.DidNotReceive().UpdateAsync(Arg.Any<Artist>());
    }

    [Fact]
    public async Task CreateAlbumAsync_Should_Throw_When_Album_Name_Is_Null_Or_Empty()
    {
        //Arange
        var artist = new Artist("Metallica");

        _artistRepository.GetByIdAsync(artist.Id).Returns(artist);


        //Act and Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _albumService.CreateAlbumAsync(new CreateAlbumRequest(artist.Id, string.Empty))
        );

        //Assert
        Assert.Equal("name", exception.ParamName);
        Assert.Contains("cannot be null or empty.", exception.Message);
        Assert.Empty(artist.Albums);

        await _artistRepository.DidNotReceive().UpdateAsync(Arg.Any<Artist>());
    }

    [Fact]
    public async Task CreateAlbumAsync_Should_Throw_When_Album_Name_Is_Same_As_Current_One()
    {
        //Arange
        var artist = new Artist("Metallica");
        artist.AddAlbum("Ride the Lightning");
        var request = new CreateAlbumRequest(artist.Id, "Ride the Lightning");

        _artistRepository.GetByIdAsync(artist.Id).Returns(artist);


        //Act and Assert
        var exception = await Assert.ThrowsAsync<DuplicateAlbumException>(
            () => _albumService.CreateAlbumAsync(request)
        );

        //Assert
        Assert.Contains($"Album '{request.Name}' already exists.", exception.Message);
        Assert.Single(artist.Albums);

        await _artistRepository.DidNotReceive().UpdateAsync(Arg.Any<Artist>());
    }

    [Fact]
    public async Task GetAlbumByIdAsync_Should_Return_AlbumDto_When_Found()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var album = artist.AddAlbum("Ride the Lightning");

        _artistRepository.GetByIdAsync(artist.Id).Returns(artist);

        //Act
        var response = await _albumService.GetAlbumByIdAsync(artist.Id, album.Id);

        //Assert
        Assert.NotNull(response);
        Assert.Equal(album.Id, response.Id);
        Assert.Equal(album.Name, response.Name);
        Assert.Equal(album.Duration, response.Duration);
    }

    [Fact]
    public async Task GetAlbumByIdAsync_Should_Throw_When_Artist_Not_Found()
    {
        //Arrange
        _artistRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((Artist?)null);

        //Act and Assert
        var exception = await Assert.ThrowsAsync<ArtistNotFoundException>(
            () => _albumService.GetAlbumByIdAsync(Guid.NewGuid(), Guid.NewGuid())
        );

        //Assert
        Assert.Contains("Artist not found.", exception.Message);
    }

    [Fact]
    public async Task GetAlbumByIdAsync_Should_Throw_When_Artist_Album_Not_Found()
    {
        //Arrange
        var artist = new Artist("Metallica");
        _artistRepository.GetByIdAsync(artist.Id).Returns(artist);

        //Act and Assert
        var exception = await Assert.ThrowsAsync<AlbumNotFoundException>(
            () => _albumService.GetAlbumByIdAsync(artist.Id, Guid.NewGuid())
        );

        //Assert
        Assert.Contains("Album not found.", exception.Message);
    }

    [Fact]
    public async Task GetAlbumsByArtist_Should_Return_Albums_When_Artist_Exists()
    {
        //Arrange
        var artist = new Artist("Metallica");
        artist.AddAlbum("Ride the Lightning");
        artist.AddAlbum("Master of Puppets");

        _artistRepository.GetByIdAsync(artist.Id).Returns(artist);

        //Act
        var response = await _albumService.GetAlbumsByArtist(artist.Id);

        //Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Count);

        Assert.Collection(response,
            album => Assert.Equal("Ride the Lightning", album.Name),
            album => Assert.Equal("Master of Puppets", album.Name)
        );

        await _artistRepository.Received(1).GetByIdAsync(artist.Id);
    }

    [Fact]
    public async Task GetAlbumsByArtist_Should_Throw_When_Artist_Not_Found()
    {
        //Arrange
        _artistRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((Artist?)null);

        //Act and Assert
        var exception = await Assert.ThrowsAsync<ArtistNotFoundException>(
            () => _albumService.GetAlbumsByArtist(Guid.NewGuid())
        );

        //Assert
        Assert.Contains("Artist not found.", exception.Message);
    }

    [Fact]
    public async Task DeleteAlbumAsync_Should_Remove_Album_And_Persist_When_Artist_And_Album_Exist()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var album = artist.AddAlbum("Ride the Lightning");
        artist.AddAlbum("Master of Puppets");
        _artistRepository.GetByIdAsync(artist.Id).Returns(artist);

        //Act and Assert
        Assert.Equal(2, artist.Albums.Count);

        await _albumService.DeleteAlbumAsync(artist.Id, album.Id);

        Assert.Single(artist.Albums);
        Assert.Equal("Master of Puppets", artist.Albums.First().Name);

        await _artistRepository.Received(1).UpdateAsync(artist);
    }

    [Fact]
    public async Task DeleteAlbumAsync_Should_Throw_And_Not_Persist_When_Artist_Not_Found()
    {
        //Arrange
        _artistRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((Artist?)null);

        //Act
        var exception = await Assert.ThrowsAsync<ArtistNotFoundException>(
           () => _albumService.DeleteAlbumAsync(Guid.NewGuid(), Guid.NewGuid())
        );

        //Assert
        Assert.Contains("Artist not found.", exception.Message);

        await _artistRepository.DidNotReceive().UpdateAsync(Arg.Any<Artist>());
    }

    [Fact]
    public async Task DeleteAlbumAsync_Should_Throw_And_Not_Persist_When_Album_Not_Found()
    {
        //Arrange
        var artist = new Artist("Metallica");
        _artistRepository.GetByIdAsync(artist.Id).Returns(artist);

        //Act
        var exception = await Assert.ThrowsAsync<AlbumNotFoundException>(
           () => _albumService.DeleteAlbumAsync(artist.Id, Guid.NewGuid())
        );

        //Assert
        Assert.Contains("Album not found.", exception.Message);

        await _artistRepository.DidNotReceive().UpdateAsync(Arg.Any<Artist>());
    }
}