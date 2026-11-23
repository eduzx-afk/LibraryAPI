namespace LibraryAPI.Models;

public class Category
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }

    public ICollection<Book> Books { get; private set; }
        = new List<Book>();

    private Category() { }

    public Category(string name, string? description = null)
    {
        UpdateName(name);
        UpdateDescription(description);
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome inválido.");

        Name = name;
    }

    public void UpdateDescription(string? description) 
        => Description = description;
}