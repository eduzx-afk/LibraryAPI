using LibraryAPI.Models;

namespace LibraryAPI.Services.Interfaces;

public interface BookService
{
    Task<List<Book>> GetAllBooksAsync();

    Task<Book?> GetBookByIdAsync(int id);

    Task<Book> CreateBookAsync(
        string title,
        string isbn,
        int publicationYear,
        int authorId,
        int categoryId
    );

    Task<bool> UpdateBookAsync(
        int id,
        string title,
        int publicationYear
    );

    Task<bool> DeleteBookAsync(int id);
}