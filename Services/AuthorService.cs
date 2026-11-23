using Microsoft.EntityFrameworkCore;
using LibraryAPI.Data;
using LibraryAPI.Models;
using LibraryAPI.Services.Interfaces;

namespace LibraryAPI.Services;

public class DefaultAuthorService(LibraryDbContext context) : AuthorService
{
    private readonly LibraryDbContext _context = context;

    public async Task<List<Author>> GetAllAuthorsAsync()
        => await _context.Authors
            .Include(a => a.Books)
                .ThenInclude(b => b.Category)
            .ToListAsync();

    public async Task<Author?> GetAuthorByIdAsync(int id)
        => await _context.Authors
            .Include(a => a.Books)
                .ThenInclude(b => b.Category)
            .FirstOrDefaultAsync(a => a.Id == id);

    public async Task<Author> CreateAuthorAsync(
        string name,
        string? biography)
    {
        var normalizedName = Normalize(name);

        bool nameExists = await _context.Authors.AnyAsync(
            a => a.Name.Trim().ToLower() == normalizedName
        );

        if (nameExists)
            throw new ArgumentException("Nome já existe.");

        var author = new Author(
            name,
            biography
        );

        _context.Authors.Add(author);
        await _context.SaveChangesAsync();

        return author;
    }

    public async Task<bool> UpdateAuthorAsync(
        int id,
        string name,
        string? biography)
    {
        var author = await _context.Authors.FindAsync(id);

        if (author == null)
            return false;

        var normalizedName = Normalize(name);

        bool nameExists = await _context.Authors.AnyAsync(
            a => a.Id != id &&
                 a.Name.Trim().ToLower() == normalizedName
        );

        if (nameExists)
            throw new ArgumentException("Nome já existe.");

        author.UpdateName(name);
        author.UpdateBiography(biography);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAuthorAsync(int id)
    {
        var author = await _context.Authors.FindAsync(id);

        if (author == null)
            return false;

        bool hasBooks = await _context.Books
            .AnyAsync(b => b.AuthorId == id);

        if (hasBooks)
            throw new InvalidOperationException(
                "Não é possível excluir um autor que possui livros cadastrados."
            );

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<Book>> GetAuthorBooksAsync(int id)
        => await _context.Books
            .Where(b => b.AuthorId == id)
            .ToListAsync();

    private static string Normalize(string value)
        => value.Trim().ToLowerInvariant();
}
