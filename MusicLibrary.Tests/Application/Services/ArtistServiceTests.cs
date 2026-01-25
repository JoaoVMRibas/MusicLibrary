using MusicLibrary.Application.Abstractions.Repositories;
using NSubstitute;

namespace MusicLibrary.Tests.Application.Services;

public class ArtistServiceTests
{
    [Fact]
    public async Task Should_Create_Artist_With_Valid_Name()
    {
        //Arrange
        var repository = Substitute.For<IArtistRepository>();
    }
}
