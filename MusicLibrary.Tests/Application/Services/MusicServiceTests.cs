using MusicLibrary.Application.Abstractions.Repositories;
using MusicLibrary.Application.Abstractions.Services;
using MusicLibrary.Application.Requests.Music;
using MusicLibrary.Application.Services;
using MusicLibrary.Domain.Entities;
using NSubstitute;

namespace MusicLibrary.Tests.Application.Services;

public class MusicServiceTests
{
    private readonly IArtistRepository _artistRepository;
    private readonly IMusicService _musicService;

    public MusicServiceTests()
    {
        _artistRepository = Substitute.For<IArtistRepository>();
        _musicService = new MusicService(_artistRepository);
    }

    [Fact]
    public async Task CreateMusicAsync_Should_Create_Music_When_Data_Is_Valid()
    {
        //Arrage
        var artist = new Artist("Metallica");
        var request = new CreateMusicRequest(artist.Id, "Fade to Black", TimeSpan.FromSeconds(415));

        _artistRepository.GetByIdAsync(request.ArtistId).Returns(artist);

        //Act
        var response = await _musicService.CreateMusicAsync(request);

        //Assert
        Assert.Single(artist.Musics);
        Assert.NotEqual(Guid.NewGuid(), response.Id);
        Assert.Equal("Fade to Black", response.Name);
        Assert.Equal(TimeSpan.FromSeconds(415),response.Duration);

        await _artistRepository.Received(1).UpdateAsync(Arg.Any<Artist>());
    }

    [Fact]
    public async Task CreateMusicAsync_Should_Throw_When_Artist_Not_Found()
    {
        //Arrage
        var request = new CreateMusicRequest(Guid.NewGuid(), "Fade to Black", TimeSpan.FromSeconds(415));

        _artistRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((Artist?)null);

        //Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _musicService.CreateMusicAsync(request));

        //Assert
        Assert.Contains("Artist not found", exception.Message);

        await _artistRepository.DidNotReceive().UpdateAsync(Arg.Any<Artist>());
    }

    [Fact]
    public async Task GetMusicByIdAsync_Should_Return_Music_When_Found()
    {
        //Arrage
        var artist = new Artist("Metallica");
        var music = artist.AddMusic("Fade to Black", TimeSpan.FromSeconds(415));
        var request = new GetMusicByIdRequest(artist.Id,music.Id);

        _artistRepository.GetByIdAsync(request.ArtistId).Returns(artist);

        //Act
        var response = await _musicService.GetMusicByIdAsync(request);

        //Assert
        Assert.NotNull(response);
        Assert.Equal(music.Id, response.Id);
        Assert.Equal(music.Name, response.Name);
        Assert.Equal(music.Duration, response.Duration);
    }

    [Fact]
    public async Task GetMusicByIdAsync_Should_Throw_When_Music_Not_Found()
    {
        //Arrage
        var artist = new Artist("Metallica");
        var request = new GetMusicByIdRequest(artist.Id, Guid.NewGuid());

        _artistRepository.GetByIdAsync(request.ArtistId).Returns(artist);

        //Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _musicService.GetMusicByIdAsync(request));

        //Assert
        Assert.Contains("Music not found", exception.Message);
    }

    [Fact]
    public async Task GetMusicsByArtistAsync_Should_Return_All_Musics()
    {
        //Arrage
        var artist = new Artist("Metallica");
        artist.AddMusic("Fade to Black", TimeSpan.FromSeconds(415));
        artist.AddMusic("The Unforgiven", TimeSpan.FromSeconds(388));
        var request = new GetMusicsByArtistRequest(artist.Id);

        _artistRepository.GetByIdAsync(request.ArtistId).Returns(artist);

        //Act
        var response = await _musicService.GetMusicsByArtistAsync(request);

        //Assert
        Assert.Equal(2, response.Count);
        Assert.Collection(response,
            music => Assert.Equal("Fade to Black", music.Name),
            music => Assert.Equal("The Unforgiven", music.Name)
        );
    }

    [Fact]
    public async Task DeleteMusicAsync_Should_Remove_Music_When_Found()
    {
        //Arrage
        var artist = new Artist("Metallica");
        var music = artist.AddMusic("Fade to Black", TimeSpan.FromSeconds(415));
        var request = new DeleteMusicRequest(artist.Id,music.Id);

        _artistRepository.GetByIdAsync(request.ArtistId).Returns(artist);

        //Act and Assert
        Assert.Single(artist.Musics);
        await _musicService.DeleteMusicAsync(request);
        Assert.Empty(artist.Musics);

        await _artistRepository.Received(1).UpdateAsync(Arg.Any<Artist>());
    }

    [Fact]
    public async Task DeleteMusicAsync_Should_Throw_When_Music_Not_Found()
    {
        //Arrage
        var artist = new Artist("Metallica");
        var request = new DeleteMusicRequest(artist.Id, Guid.NewGuid());

        _artistRepository.GetByIdAsync(request.ArtistId).Returns(artist);

        //Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _musicService.DeleteMusicAsync(request));

        //Assert
        Assert.Contains("Music not found", exception.Message);

        await _artistRepository.DidNotReceive().UpdateAsync(Arg.Any<Artist>());
    }

    [Fact]
    public async Task AddMusicToAlbumAsync_Should_Add_Music_To_Album()
    {
        //Arrage
        var artist = new Artist("Metallica");
        var album = artist.AddAlbum("Ride the Lightning");
        var music = artist.AddMusic("Fade to Black", TimeSpan.FromSeconds(415));
        var request = new AddMusicToAlbumRequest(artist.Id, album.Id,music.Id);

        _artistRepository.GetByIdAsync(request.ArtistId).Returns(artist);

        //Act
        await _musicService.AddMusicToAlbumAsync(request);

        //Assert
        Assert.Single(album.Musics);
        Assert.Equal(music.Id, album.Musics.First().Id);
        Assert.Equal(music.Name,album.Musics.First().Name);
        Assert.Equal(music.Duration,album.Musics.First().Duration);

        await _artistRepository.Received(1).UpdateAsync(Arg.Any<Artist>());
    }
}
