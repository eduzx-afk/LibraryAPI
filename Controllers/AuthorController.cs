using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Models;
using LibraryAPI.Services.Interfaces;
using LibraryAPI.DTOs.Authors;

[ApiController]
[Route("api/[controller]")]
public class AuthorController(AuthorService service) : ControllerBase
{
	private readonly AuthorService _service = service;

	// GET: api/author
	[HttpGet]
	public async Task<ActionResult<List<AuthorResponseDTO>>> GetAllAuthors()
	{
		var authors = await _service.GetAllAuthorsAsync();

		var response = authors.Select(MapToDTO).ToList();

		return Ok(response);
	}

	// GET: api/author/5
	[HttpGet("{id:int}")]
	public async Task<ActionResult<AuthorResponseDTO>> GetAuthorById(int id)
	{
		var author = await _service.GetAuthorByIdAsync(id);

		if (author == null)
			return NotFound();

		return Ok(MapToDTO(author));
	}

	// POST: api/author
	[HttpPost]
	public async Task<ActionResult<AuthorResponseDTO>> CreateBook(
		AuthorRequestDTO dto
	)
	{
		try
		{
			var author = await _service.CreateAuthorAsync(
				dto.Name,
				dto.Biography
			);

			return CreatedAtAction(
				nameof(GetAuthorById),
				new { id = author.Id },
				MapToDTO(author)
			);
		}
		catch (ArgumentException ex)
		{
			return Conflict(new { message = ex.Message });
		}
	}

	// PUT: api/book/5
	[HttpPut("{id:int}")]
	public async Task<IActionResult> UpdateAuthor(
		int id,
		AuthorRequestDTO dto
	)
	{
		try 
		{
			bool updated = await _service.UpdateAuthorAsync(
				id,
				dto.Name,
				dto.Biography
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

	// DELETE: api/author/5
	[HttpDelete("{id:int}")]
	public async Task<IActionResult> DeleteAuthor(int id)
	{
		try
		{
			bool deleted = await _service.DeleteAuthorAsync(id);

			if (!deleted)
				return NotFound();

			return NoContent();
		}
		catch (InvalidOperationException ex)
		{
			return Conflict(new { message = ex.Message });
		}
	}

    // GET: api/author/5/books
    [HttpGet("{id:int}/books")]
    public async Task<ActionResult<List<AuthorBookDTO>>> GetAuthorBooks(int id)
    {
        var author = await _service.GetAuthorByIdAsync(id);

        if (author == null)
            return NotFound();

        var books = await _service.GetAuthorBooksAsync(id);

        var response = books.Select(book => new AuthorBookDTO(
            book.Id,
            book.Title,
            book.ISBN,
            book.PublicationYear,
            book.Category.Name,
            book.IsAvailable
        )).ToList();

        return Ok(response);
    }

	private static AuthorResponseDTO MapToDTO(Author author)
		=> new AuthorResponseDTO(
			author.Id,
            author.Name,
            author.Biography,
            author.Books.Select(book => new AuthorBookDTO(
                book.Id,
                book.Title,
                book.ISBN,
                book.PublicationYear,
                book.Category.Name,
                book.IsAvailable
            )).ToList()
		);
}
