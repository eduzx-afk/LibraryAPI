using Microsoft.EntityFrameworkCore;
using LibraryAPI.Data;
using LibraryAPI.Models;
using LibraryAPI.Services.Interfaces;


namespace LibraryAPI.Services;

public class DefaultBookService(LibraryDbContext context) : BookService
{
    private readonly LibraryDbContext _context = context;

    public async Task<List<Book>> GetAllBooksAsync()
        => await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Category)
            .ToListAsync();

    public async Task<Book> GetBookByIdAsync(int id)
        => await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Category)
            .FirstOrDefaultAsync(b => b.Id == id);

    public async Task<Book> CreateBookAsync(
        string title,
        string isbn,
        int publicationYear,
        int authorId,
        int categoryId)
    {
        isbn = isbn.Replace("-", "").Replace(" ", "");

        if (!IsValidISBN13(isbn))
            throw new ArgumentException("ISBN-13 inválida");

        bool isbnExists = await _context.Books.AnyAsync(b => b.ISBN == isbn);
        if (isbnExists) 
            throw new InvalidOperationException("ISBN já existe.");

        var author = await _context.Authors.FindAsync(authorId);
        var category = await _context.Categories.FindAsync(categoryId);

        if (author == null) 
            throw new KeyNotFoundException("Autor não encontrado.");
        if (category == null)
            throw new KeyNotFoundException("Categoria não encontrada.");

        var book = new Book(
            title,
            isbn,
            publicationYear,
            author,
            category
        );

        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        return book;
    }

    private bool IsValidISBN13(string isbn)
    {
        if (isbn.Length != 13 || !isbn.All(char.IsDigit))
            return false;
        
        int sum = 0;

        for (int i = 0; i < 12; i++)
        {
            int digit = isbn[i] - '0';
            sum = digit * (i % 2 == 0 ? 1 : 3);
        }

        int checkDigit = (10 - (sum % 10)) % 10;

        return checkDigit == (isbn[12] - '0');
    }

    public async Task<bool> UpdateBookAsync(
        int id, 
        string title, 
        int publicationYear)
    {
        var book = await _context.Books.FindAsync(id);

        if (book == null)
            return false;

        book.UpdateTitle(title);
        book.UpdatePublicationYear(publicationYear);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);

        if (book == null)
            return false;

        if (!book.IsAvailable)
            throw new InvalidOperationException(
                "Livro emprestado não pode ser deletado."
            );

        _context.Books.Remove(book);

        await _context.SaveChangesAsync();

        return true;
    }
}