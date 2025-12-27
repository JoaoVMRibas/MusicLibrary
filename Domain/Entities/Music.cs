namespace MusicLibrary.Domain.Entities;

public class Music
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    internal Music(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("The music name cannot be null or empty.", nameof(name));

        Id = Guid.NewGuid();
        Name = name;
    }
}
