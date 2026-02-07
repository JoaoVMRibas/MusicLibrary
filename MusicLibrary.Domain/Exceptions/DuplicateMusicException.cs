namespace MusicLibrary.Domain.Exceptions;

public sealed class DuplicateMusicException : Exception
{
    public DuplicateMusicException(string name) 
        : base($"Music '{name}' already exists.") { }
}