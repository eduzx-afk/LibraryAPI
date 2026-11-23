using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Models;
using LibraryAPI.Services.Interfaces;
using LibraryAPI.DTOs.Books;

[ApiController]
[Route("api/[controller]")]
public class BookController(BookService service) : ControllerBase
{
	private readonly BookService _service = service;

	// GET: api/book
	[HttpGet]
	public async Task<ActionResult<List<BookResponseDTO>>> GetAllBooks()
	{
		var books = await _service.GetAllBooksAsync();

		var response = books.Select(MapToDTO).ToList();

		return Ok(response);
	}

	// GET: api/book/5
	[HttpGet("{id:int}")]
	public async Task<ActionResult<BookResponseDTO>> GetBookById(int id)
	{
		var book = await _service.GetBookByIdAsync(id);

		if (book == null)
			return NotFound();

		return Ok(MapToDTO(book));
	}

	// POST: api/book
	[HttpPost]
	public async Task<ActionResult<BookResponseDTO>> CreateBook(
		CreateBookDTO dto
	)
	{
		try
		{
			var book = await _service.CreateBookAsync(
				dto.Title,
				dto.ISBN,
				dto.PublicationYear,
				dto.AuthorId,
				dto.CategoryId
			);

			var response = MapToDTO(book);

			return CreatedAtAction(
				nameof(GetBookById),
				new { id = book.Id },
				response
			);
		}
		catch (ArgumentException ex)
		{
			return BadRequest(new { message = ex.Message });
		}
		catch (KeyNotFoundException ex)
		{
			return NotFound(new { message = ex.Message });
		}
		catch (InvalidOperationException ex)
		{
			return Conflict(new { message = ex.Message });
		}
	}

	// PUT: api/book/5
	[HttpPut("{id:int}")]
	public async Task<IActionResult> UpdateBook(
		int id,
		UpdateBookDTO dto
	)
	{
		try 
		{
			bool updated = await _service.UpdateBookAsync(
				id,
				dto.Title,
				dto.PublicationYear
			);

			if (!updated)
				return NotFound();

			return NoContent();
		}
		catch (ArgumentException ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}

	// DELETE: api/book/5
	[HttpDelete("{id:int}")]
	public async Task<IActionResult> DeleteBook(int id)
	{
		try
		{
			bool deleted = await _service.DeleteBookAsync(id);

			if (!deleted)
				return NotFound();

			return NoContent();
		}
		catch (InvalidOperationException ex)
		{
			return Conflict(new { message = ex.Message });
		}
	}

	private static BookResponseDTO MapToDTO(Book book)
		=> new BookResponseDTO(
			book.Id,
			book.Title,
			book.ISBN,
			book.PublicationYear,
			book.Author.Name,
			book.Category.Name,
			book.IsAvailable
		);
}
