namespace MusicLibrary.Domain.Exceptions;

public sealed class MusicNotFoundException : Exception
{
    public MusicNotFoundException()
    : base("Music not found.") { }
}
