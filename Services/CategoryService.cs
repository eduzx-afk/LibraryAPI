using Microsoft.EntityFrameworkCore;
using LibraryAPI.Data;
using LibraryAPI.Models;
using LibraryAPI.Services.Interfaces;

namespace LibraryAPI.Services;

public class DefaultCategoryService(LibraryDbContext context) : CategoryService
{
    private readonly LibraryDbContext _context = context;

    public async Task<List<Category>> GetAllCategoriesAsync()
        => await _context.Categories
            .Include(c => c.Books)
                .ThenInclude(b => b.Author)
            .ToListAsync();

    public async Task<Category?> GetCategoryByIdAsync(int id)
        => await _context.Categories
            .Include(c => c.Books)
                .ThenInclude(b => b.Author)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<Category> CreateCategoryAsync(
        string name,
        string? description)
    {
        var normalizedName = Normalize(name);

        bool nameExists = await _context.Categories.AnyAsync(
            c => c.Name.Trim().ToLower() == normalizedName
        );

        if (nameExists)
            throw new ArgumentException("Categoria já existe.");

        var category = new Category(
            name,
            description
        );

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return category;
    }

    public async Task<bool> UpdateCategoryAsync(
        int id,
        string name,
        string? description)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
            return false;

        var normalizedName = Normalize(name);

        bool nameExists = await _context.Categories.AnyAsync(
            c => c.Id != id &&
                 c.Name.Trim().ToLower() == normalizedName
        );

        if (nameExists)
            throw new ArgumentException("Categoria já existe.");

        category.UpdateName(name);
        category.UpdateDescription(description);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
            return false;

        bool hasBooks = await _context.Books
            .AnyAsync(b => b.CategoryId == id);

        if (hasBooks)
            throw new InvalidOperationException(
                "Não é possível excluir uma categoria que possui livros cadastrados."
            );

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<Book>> GetCategoryBooksAsync(int id)
        => await _context.Books
            .Where(b => b.CategoryId == id)
            .ToListAsync();

    private static string Normalize(string value)
        => value.Trim().ToLowerInvariant();
}
