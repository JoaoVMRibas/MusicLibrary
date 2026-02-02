namespace MusicLibrary.Domain.Exceptions;

public sealed class MusicAlreadyInAlbumException : Exception
{
    public MusicAlreadyInAlbumException() 
        : base("Music already added to album.") { }
}
