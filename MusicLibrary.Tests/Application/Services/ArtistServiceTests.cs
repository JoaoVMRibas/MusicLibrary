using MusicLibrary.Application.Abstractions.Repositories;
using MusicLibrary.Application.Requests.Artist;
using MusicLibrary.Application.Services;
using MusicLibrary.Domain.Entities;
using NSubstitute;

namespace MusicLibrary.Tests.Application.Services;

public class ArtistServiceTests
{
    private readonly IArtistRepository _artistRepository;
    private readonly ArtistService _artistService;

    public ArtistServiceTests()
    {
        _artistRepository = Substitute.For<IArtistRepository>();
        _artistService = new ArtistService(_artistRepository);
    }

    [Fact]
    public async Task CreateAsync_Should_Create_Artist_And_Save_In_Repository()
    {
        //Arrange
        var request = new CreateArtistRequest("Metallica");

        //Act
        var response = await _artistService.CreateAsync(request);

        //Assert
        await _artistRepository.Received(1).AddAsync(Arg.Is<Artist>(a => a.Name == "Metallica"));
        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.Equal("Metallica", response.Name);
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_Exception_When_Name_Is_Invalid()
    {
        //Arrange
        var request = new CreateArtistRequest(string.Empty);

        //Act and Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _artistService.CreateAsync(request));

        Assert.Equal("name", exception.ParamName);

        await _artistRepository.DidNotReceive().AddAsync(Arg.Any<Artist>());
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Artist_When_Found()
    {
        //Arrange
        var artist = new Artist("Metallica");

        _artistRepository.GetByIdAsync(artist.Id).Returns(artist);

        //Act
        var response = await _artistService.GetByIdAsync(new GetArtistByIdRequest(artist.Id));

        //Assert
        Assert.NotNull(response);
        Assert.Equal(artist.Id, response.Id);
        Assert.Equal(artist.Name, response.Name);

        await _artistRepository.Received(1).GetByIdAsync(artist.Id);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Throw_Exception_When_Artist_Not_Found()
    {
        //Arrange
        var artistId = Guid.NewGuid();

        _artistRepository.GetByIdAsync(artistId).Returns((Artist?)null);

        //Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _artistService.GetByIdAsync(new GetArtistByIdRequest(artistId)));

        //Assert
        Assert.Contains("Artist not found.", exception.Message);
        await _artistRepository.Received(1).GetByIdAsync(artistId);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_Empty_List_When_No_Artists_Exist()
    {
        //Arrange
        _artistRepository.GetAllAsync().Returns(new List<Artist>());

        //Act
        var response = await _artistService.GetAllAsync();

        //Assert
        Assert.NotNull(response);
        Assert.Empty(response);

        await _artistRepository.Received(1).GetAllAsync();
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_Artists_As_ArtistDto()
    {
        //Arrange
        _artistRepository.GetAllAsync()
                    .Returns(
                        new List<Artist>
                        {
                            new Artist("Metallica"),
                            new Artist("Guns")
                        }
                    );

        //Act
        var response = await _artistService.GetAllAsync();

        //Assert
        Assert.Equal(2, response.Count);
        Assert.All(response, a => Assert.NotEqual(Guid.Empty, a.Id));
        Assert.Contains(response, a => a.Name == "Metallica");
        Assert.Contains(response, a => a.Name == "Guns");

        await _artistRepository.Received(1).GetAllAsync();
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Artist_When_Data_Is_Valid()
    {
        //Arrange
        var artist = new Artist("Iron Maiden");
        var request = new UpdateArtistRequest(artist.Id, "Guns");

        _artistRepository.GetByIdAsync(artist.Id).Returns(artist);

        //Act
        var response = await _artistService.UpdateAsync(request);

        //Assert
        Assert.Equal(artist.Id, response.Id);
        Assert.Equal("Guns", response.Name);

        await _artistRepository.Received(1).UpdateAsync(artist);
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Artist_Not_Found()
    {
        //Arrange
        var artistId = Guid.NewGuid();
        var request = new UpdateArtistRequest(artistId, "Guns");

        _artistRepository.GetByIdAsync(artistId).Returns((Artist?)null);

        //Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _artistService.UpdateAsync(request));

        //Assert
        Assert.Contains("Artist not found.", exception.Message);

        await _artistRepository.DidNotReceive().UpdateAsync(Arg.Any<Artist>());
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Updated_Name_Is_Invalid()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var request = new UpdateArtistRequest(artist.Id, string.Empty);

        _artistRepository.GetByIdAsync(artist.Id).Returns(artist);

        //Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _artistService.UpdateAsync(request));

        //Assert
        Assert.Equal("name", exception.ParamName);
        Assert.Contains("The artist's name cannot be null or empty.", exception.Message);

        await _artistRepository.DidNotReceive().UpdateAsync(Arg.Any<Artist>());
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Updated_Name_Is_The_Same_As_The_Actual_Name()
    {
        //Arrange
        var artist = new Artist("Metallica");
        var request = new UpdateArtistRequest(artist.Id, "Metallica");

        _artistRepository.GetByIdAsync(artist.Id).Returns(artist);

        //Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _artistService.UpdateAsync(request));

        //Assert
        Assert.Contains("The artist already has this name.", exception.Message);

        await _artistRepository.DidNotReceive().UpdateAsync(Arg.Any<Artist>());
    }

    [Fact]
    public async Task DeleteAsync_Should_Delete_Artist_When_Exists()
    {
        //Arrange
        var artist = new Artist("Metallica");

        _artistRepository.GetByIdAsync(artist.Id).Returns(artist);

        //Act
        await _artistService.DeleteAsync(new DeleteArtistRequest(artist.Id));

        //Assert
        await _artistRepository.Received(1).DeleteAsync(artist);
    }

    [Fact]
    public async Task DeleteAsync_Should_Throw_When_Artist_Not_Found()
    {
        //Arrange
        _artistRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((Artist?)null);

        //Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _artistService.DeleteAsync(new DeleteArtistRequest(Guid.NewGuid())));

        //Assert
        Assert.Contains("Artist not found.", exception.Message);

        await _artistRepository.DidNotReceive().DeleteAsync(Arg.Any<Artist>());
    }
}
