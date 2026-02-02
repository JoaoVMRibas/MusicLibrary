namespace MusicLibrary.Domain.Exceptions;

public sealed class ArtistAlreadyHasThisNameException : Exception
{
    public ArtistAlreadyHasThisNameException(string name)
        : base($"The artist already has the name '{name}'.") { }
}
