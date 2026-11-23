using LibraryAPI.Models;

namespace LibraryAPI.Services.Interfaces;


public interface AuthorService
{
    Task<List<Author>> GetAllAuthorsAsync();

    Task<Author?> GetAuthorByIdAsync(int id);

    Task<Author> CreateAuthorAsync(
        string name,
        string? biography
    );
    
    Task<bool> UpdateAuthorAsync(
        int id,
        string name,
        string? biography
    );

    Task<bool> DeleteAuthorAsync(int id);

    Task<List<Book>> GetAuthorBooksAsync(int id);
}
