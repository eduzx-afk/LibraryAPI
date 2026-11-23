using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Models;
using LibraryAPI.Services.Interfaces;
using LibraryAPI.DTOs.Categories;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(CategoryService service) : ControllerBase
{
    private readonly CategoryService _service = service;

    // GET: api/category
    [HttpGet]
    public async Task<ActionResult<List<CategoryResponseDTO>>> GetAllCategories()
    {
        var categories = await _service.GetAllCategoriesAsync();
        var response = categories.Select(MapToDTO).ToList();

        return Ok(response);
    }

    // GET: api/category/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryResponseDTO>> GetCategoryById(int id)
    {
        var category = await _service.GetCategoryByIdAsync(id);

        if (category == null)
            return NotFound();

        return Ok(MapToDTO(category));
    }

    // POST: api/category
    [HttpPost]
    public async Task<ActionResult<CategoryResponseDTO>> CreateCategory(
        CategoryRequestDTO dto)
    {
        try
        {
            var category = await _service.CreateCategoryAsync(
                dto.Name,
                dto.Description
            );

            return CreatedAtAction(
                nameof(GetCategoryById),
                new { id = category.Id },
                MapToDTO(category)
            );
        }
        catch (ArgumentException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // PUT: api/category/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCategory(
        int id,
        CategoryRequestDTO dto)
    {
        try
        {
            bool updated = await _service.UpdateCategoryAsync(
                id,
                dto.Name,
                dto.Description
            );

            if (!updated)
                return NotFound();

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // DELETE: api/category/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        try
        {
            bool deleted = await _service.DeleteCategoryAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // GET: api/category/5/books
    [HttpGet("{id:int}/books")]
    public async Task<ActionResult<List<CategoryBookDTO>>> GetCategoryBooks(int id)
    {
        var category = await _service.GetCategoryByIdAsync(id);

        if (category == null)
            return NotFound();

        var books = await _service.GetCategoryBooksAsync(id);

        var response = books.Select(book => new CategoryBookDTO(
            book.Id,
            book.Title,
            book.ISBN,
            book.PublicationYear,
            book.Author.Name,
            book.IsAvailable
        )).ToList();

        return Ok(response);
    }

    private static CategoryResponseDTO MapToDTO(Category category)
        => new(
            category.Id,
            category.Name,
            category.Description,
            category.Books.Select(book => new CategoryBookDTO(
                book.Id,
                book.Title,
                book.ISBN,
                book.PublicationYear,
                book.Author.Name,
                book.IsAvailable
            )).ToList()
        );
}
