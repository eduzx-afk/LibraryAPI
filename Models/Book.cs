namespace LibraryAPI.Models;

public class Book
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string ISBN { get; private set; }
    public int PublicationYear { get; private set; }

    public int AuthorId { get; private set; }
    public Author? Author { get; private set; } = null;

    public int CategoryId { get; private set; }
    public Category? Category { get; private set; } = null;

    public bool IsAvailable { get; private set; } = true;

    private Book() { } 

    public Book(string title, string isbn, int publicationYear,
                Author author, Category category)
    {
        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("ISBN inválido.");

        UpdateTitle(title);
        UpdatePublicationYear(publicationYear);

        ISBN = isbn;
        Author = author;
        Category = category;
    }

    public void UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Título inválido.");

        Title = title;
    }

    public void UpdatePublicationYear(int year)
    {
        if (year <= 0)
            throw new ArgumentException(
                "Ano de publicação inválido.");

        PublicationYear = year;
    }

    internal void Borrow()
    {
        if (!IsAvailable)
            throw new InvalidOperationException(
                "O livro não está disponível!");

        IsAvailable = false;
    }

    internal void Return() => IsAvailable = true;
}
