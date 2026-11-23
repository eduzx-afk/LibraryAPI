namespace LibraryAPI.Models;

public class Author
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string? Biography { get; private set; }

    public ICollection<Book> Books { get; private set; }
        = new List<Book>();

    private Author() { } 

    public Author(string name, string? biography = null)
    {
        UpdateName(name);
        UpdateBiography(biography);
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome inválido.");

        Name = name;
    }

    public void UpdateBiography(string? biography) 
        => Biography = biography;
}