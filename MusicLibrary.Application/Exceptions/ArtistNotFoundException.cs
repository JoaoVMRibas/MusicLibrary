namespace MusicLibrary.Application.Exceptions;

public sealed class ArtistNotFoundException : Exception
{
    public ArtistNotFoundException() 
        : base("Artist not found.") { }
}
