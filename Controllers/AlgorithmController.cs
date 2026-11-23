using Microsoft.AspNetCore.Mvc;
using LibraryAPI.DTOs.Books;
using LibraryAPI.Models;
using LibraryAPI.Services.Interfaces;
using LibraryAPI.Enums;

namespace LibraryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlgorithmController(AlgorithmService service) : ControllerBase
{
    private readonly AlgorithmService _service = service;

    // GET: api/algorithm/search/linear/9780132350884
    [HttpGet("search/linear/{isbn}")]
    public async Task<ActionResult<BookResponseDTO>> LinearSearch(string isbn)
    {
        var book = await _service.LinearSearchByISBNAsync(isbn);

        if (book == null)
            return NotFound();

        return Ok(MapToDTO(book));
    }

    // GET: api/algorithm/search/binary/9780132350884
    [HttpGet("search/binary/{isbn}")]
    public async Task<ActionResult<BookResponseDTO>> BinarySearch(string isbn)
    {
        var book = await _service.BinarySearchByISBNAsync(isbn);

        if (book == null)
            return NotFound();

        return Ok(MapToDTO(book));
    }

    // GET: api/algorithm/sort?criteria=Title
    [HttpGet("sort")]
    public async Task<ActionResult<List<BookResponseDTO>>> SortBooks(
        [FromQuery] BookSortCriteria criteria)
    {
        if (!Enum.IsDefined(typeof(BookSortCriteria), criteria))
            return BadRequest(new { message = "Critério de ordenação inválido." });

        var books = await _service.SortBooksAsync(criteria);

        var response = books.Select(MapToDTO).ToList();

        return Ok(response);
    }

    // GET: api/algorithm/quickselect?k=2&criteria=PublicationYear
    [HttpGet("selection/quickselect")]
    public async Task<ActionResult<BookResponseDTO>> QuickSelect(
        [FromQuery] int k,
        [FromQuery] BookSortCriteria criteria)
    {
        if (!Enum.IsDefined(typeof(BookSortCriteria), criteria))
        return BadRequest("Critério de seleção inválido.");

    try
    {
        var book = await _service.QuickSelectAsync(k, criteria);

        if (book == null)
            return NotFound();

        return Ok(MapToDTO(book));
    }
    catch (ArgumentOutOfRangeException ex)
    {
        return BadRequest();
    }
    }

    private static BookResponseDTO MapToDTO(Book book)
    {
        return new BookResponseDTO(
            book.Id,
            book.Title,
            book.ISBN,
            book.PublicationYear,
            book.Author.Name,
            book.Category.Name,
            book.IsAvailable
        );
    }
}