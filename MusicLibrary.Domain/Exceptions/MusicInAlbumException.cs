namespace MusicLibrary.Domain.Exceptions;

public sealed class MusicInAlbumException : Exception
{
    public MusicInAlbumException() 
        : base("The music is part of an album and cannot be removed.") { }
}