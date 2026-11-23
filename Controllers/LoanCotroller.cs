using Microsoft.AspNetCore.Mvc;
using LibraryAPI.DTOs.Loans;
using LibraryAPI.Models;
using LibraryAPI.Services.Interfaces;

namespace LibraryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoanController(LoanService service) : ControllerBase
{
    private readonly LoanService _service = service;

    // GET: api/loan
    [HttpGet]
    public async Task<ActionResult<List<LoanResponseDTO>>> GetActiveLoans()
    {
        var loans = await _service.GetActiveLoansAsync();

        var response = loans.Select(MapToDTO).ToList();

        return Ok(response);
    }

    // GET: api/loan/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<LoanResponseDTO>> GetLoanById(int id)
    {
        var loan = await _service.GetLoanByIdAsync(id);

        if (loan == null)
            return NotFound();

        return Ok(MapToDTO(loan));
    }

    // GET: api/loan/user/5
    [HttpGet("user/{userId:int}")]
    public async Task<ActionResult<List<LoanResponseDTO>>> GetLoanHistory(int userId)
    {
        var loans = await _service.GetLoanHistoryAsync(userId);

        var response = loans.Select(MapToDTO).ToList();

        return Ok(response);
    }

    // POST: api/loan
    [HttpPost]
    public async Task<ActionResult<LoanResponseDTO>> CreateLoan(
        LoanRequestDTO dto)
    {
        try
        {
            var loan = await _service.BorrowBookAsync(
                dto.UserId,
                dto.BookId
            );

            return CreatedAtAction(
                nameof(GetLoanById),
                new { id = loan.Id },
                MapToDTO(loan)
            );
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT: api/loan/5/return
    [HttpPut("{id:int}/return")]
    public async Task<IActionResult> ReturnLoan(int id)
    {
        try
        {
            bool returned = await _service.ReturnBookAsync(id);

            if (!returned)
                return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    private static LoanResponseDTO MapToDTO(Loan loan)
    {
        return new LoanResponseDTO(
            loan.Id,
            loan.User.Name,
            loan.Book.Title,
            loan.Book.ISBN,
            loan.LoanDate,
            loan.DueDate,
            loan.ReturnDate,
            loan.IsActive
        );
    }
}