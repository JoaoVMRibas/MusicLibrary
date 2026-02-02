namespace MusicLibrary.Domain.Exceptions;

public sealed class DuplicateAlbumException : Exception
{
    public DuplicateAlbumException(string name) 
        : base($"Album '{name}' already exists.") { }
}
