namespace MusicLibrary.Domain.Exceptions;

public sealed class AlbumNotFoundException : Exception
{
    public AlbumNotFoundException() 
        : base("Album not found.") { }
}