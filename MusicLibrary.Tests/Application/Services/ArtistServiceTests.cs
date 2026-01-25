using MusicLibrary.Application.Abstractions.Repositories;
using MusicLibrary.Application.Requests;
using MusicLibrary.Application.Services;
using MusicLibrary.Domain.Entities;
using NSubstitute;
using System;

namespace MusicLibrary.Tests.Application.Services;

public class ArtistServiceTests
{
    [Fact]
    public async Task CreateAsync_Should_Create_Artist_And_Save_In_Repository()
    {
        //Arrange
        var repository = Substitute.For<IArtistRepository>();
        var service = new ArtistService(repository);
        var request = new CreateArtistRequest("Metallica");

        //Act
        var response = await service.CreateAsync(request);

        //Assert
        await repository.Received(1).AddAsync(Arg.Is<Artist>(a => a.Name == "Metallica"));
        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.Equal("Metallica", response.Name);
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_Exception_When_Name_Is_Invalid()
    {
        //Arrange
        var repository = Substitute.For<IArtistRepository>();
        var service = new ArtistService(repository);
        var request = new CreateArtistRequest(string.Empty);

        //Act and Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(request));

        Assert.Equal("name", exception.ParamName);

        await repository.DidNotReceive().AddAsync(Arg.Any<Artist>());
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Artist_When_Found()
    {
        //Arrange
        var repository = Substitute.For<IArtistRepository>();
        var service = new ArtistService(repository);
        var artist = new Artist("Metallica");

        repository.GetByIdAsync(artist.Id).Returns(artist);

        //Act
        var response = await service.GetByIdAsync(artist.Id);

        //Assert
        Assert.NotNull(response);
        Assert.Equal(artist.Id, response.Id);
        Assert.Equal(artist.Name, response.Name);

        await repository.Received(1).GetByIdAsync(artist.Id);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Throw_Exception_When_Artist_Not_Found()
    {
        //Arrange
        var repository = Substitute.For<IArtistRepository>();
        var service = new ArtistService(repository);
        var artistId = Guid.NewGuid();

        repository.GetByIdAsync(artistId).Returns((Artist?)null);

        //Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetByIdAsync(artistId));

        //Assert
        Assert.Contains("Artist not found.", exception.Message);
        await repository.Received(1).GetByIdAsync(artistId);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_Empty_List_When_No_Artists_Exist()
    {
        //Arrange
        var repository = Substitute.For<IArtistRepository>();
        var service = new ArtistService(repository);

        repository.GetAllAsync().Returns(new List<Artist>());

        //Act
        var response = await service.GetAllAsync();

        //Assert
        Assert.NotNull(response);
        Assert.Empty(response);

        await repository.Received(1).GetAllAsync();
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_Artists_As_ArtistDto()
    {
        //Arrange
        var repository = Substitute.For<IArtistRepository>();
        var service = new ArtistService(repository);

        repository.GetAllAsync()
                    .Returns(
                        new List<Artist>
                        {
                            new Artist("Metallica"),
                            new Artist("Guns")
                        }
                    );

        //Act
        var response = await service.GetAllAsync();

        //Assert
        Assert.Equal(2, response.Count);
        Assert.All(response, a => Assert.NotEqual(Guid.Empty, a.Id));
        Assert.Contains(response, a => a.Name == "Metallica");
        Assert.Contains(response, a => a.Name == "Guns");

        await repository.Received(1).GetAllAsync();
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Artist_When_Data_Is_Valid()
    {
        //Arrange
        var repository = Substitute.For<IArtistRepository>();
        var service = new ArtistService(repository);

        var artist = new Artist("Iron Maiden");
        var request = new UpdateArtistRequest(artist.Id, "Guns");

        repository.GetByIdAsync(artist.Id).Returns(artist);

        //Act
        var response = await service.UpdateAsync(request);

        //Assert
        Assert.Equal(artist.Id, response.Id);
        Assert.Equal("Guns", response.Name);

        await repository.Received(1).UpdateAsync(artist);
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Artist_Not_Found()
    {
        //Arrange
        var repository = Substitute.For<IArtistRepository>();
        var service = new ArtistService(repository);

        var artistId = Guid.NewGuid();
        var request = new UpdateArtistRequest(artistId, "Guns");

        repository.GetByIdAsync(artistId).Returns((Artist?)null);

        //Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateAsync(request));

        //Assert
        Assert.Contains("Artist not found.", exception.Message);

        await repository.DidNotReceive().UpdateAsync(Arg.Any<Artist>());
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Updated_Name_Is_Invalid()
    {
        //Arrange
        var repository = Substitute.For<IArtistRepository>();
        var service = new ArtistService(repository);

        var artist = new Artist("Metallica");
        var request = new UpdateArtistRequest(artist.Id, "");

        repository.GetByIdAsync(artist.Id).Returns(artist);

        //Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateAsync(request));

        //Assert
        Assert.Equal("name", exception.ParamName);
        Assert.Contains("The artist's name cannot be null or empty.", exception.Message);

        await repository.DidNotReceive().UpdateAsync(Arg.Any<Artist>());
    }

    [Fact]
    public async Task DeleteAsync_Should_Delete_Artist_When_Exists()
    {
        //Arrange
        var repository = Substitute.For<IArtistRepository>();
        var service = new ArtistService(repository);
        var artist = new Artist("Metallica");

        repository.GetByIdAsync(artist.Id).Returns(artist);

        //Act
        await service.DeleteAsync(artist.Id);

        //Assert
        await repository.Received(1).DeleteAsync(artist);
    }

    [Fact]
    public async Task DeleteAsync_Should_Throw_When_Artist_Not_Found()
    {
        //Arrange
        var repository = Substitute.For<IArtistRepository>();
        var service = new ArtistService(repository);

        repository.GetByIdAsync(Arg.Any<Guid>()).Returns((Artist?)null);

        //Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>service.DeleteAsync(Guid.NewGuid()));

        //Assert
        Assert.Contains("Artist not found.", exception.Message);

        await repository.DidNotReceive().DeleteAsync(Arg.Any<Artist>());
    }
}
