namespace MusicLibrary.Domain.Entities;

public class Album
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    internal Album(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("The album name cannot be null or empty.", nameof(name));

        Id = Guid.NewGuid();
        Name = name;
    }   
}
