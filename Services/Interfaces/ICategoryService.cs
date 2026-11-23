using LibraryAPI.Models;

namespace LibraryAPI.Services.Interfaces;

public interface CategoryService
{
    Task<List<Category>> GetAllCategoriesAsync();

    Task<Category?> GetCategoryByIdAsync(int id);

    Task<Category> CreateCategoryAsync(
        string name,
        string? description
    );

    Task<bool> UpdateCategoryAsync(
        int id,
        string name,
        string? description
    );

    Task<bool> DeleteCategoryAsync(int id);

    Task<List<Book>> GetCategoryBooksAsync(int id);
}
