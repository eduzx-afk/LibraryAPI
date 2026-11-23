using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Models;
using LibraryAPI.Services.Interfaces;
using LibraryAPI.DTOs.Users;

[ApiController]
[Route("api/[controller]")]
public class UserController(UserService service) : ControllerBase
{
    private readonly UserService _service = service;

    // GET: api/user
    [HttpGet]
    public async Task<ActionResult<List<UserResponseDTO>>> GetAllUsers()
    {
        var users = await _service.GetAllUsersAsync();
        var response = users.Select(MapToDTO).ToList();

        return Ok(response);
    }

    // GET: api/user/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserResponseDTO>> GetUserById(int id)
    {
        var user = await _service.GetUserByIdAsync(id);

        if (user == null)
            return NotFound();

        return Ok(MapToDTO(user));
    }

    // POST: api/user
    [HttpPost]
    public async Task<ActionResult<UserResponseDTO>> CreateUser(
        UserRequestDTO dto)
    {
        try
        {
            var user = await _service.CreateUserAsync(
                dto.Name,
                dto.Email
            );

            return CreatedAtAction(
                nameof(GetUserById),
                new { id = user.Id },
                MapToDTO(user)
            );
        }
        catch (ArgumentException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // PUT: api/user/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateUser(
        int id,
        UserRequestDTO dto)
    {
        try
        {
            bool updated = await _service.UpdateUserAsync(
                id,
                dto.Name,
                dto.Email
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

    // DELETE: api/user/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            bool deleted = await _service.DeleteUserAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // GET: api/user/5/loans
    [HttpGet("{id:int}/loans")]
    public async Task<ActionResult<List<UserLoanDTO>>> GetUserLoans(int id)
    {
        var user = await _service.GetUserByIdAsync(id);

        if (user == null)
            return NotFound();

        var loans = await _service.GetUserLoansAsync(id);

        var response = loans.Select(loan => new UserLoanDTO(
            loan.Id,
            loan.Book.Title,
            loan.LoanDate,
            loan.DueDate,
            loan.IsActive
        )).ToList();

        return Ok(response);
    }

    private static UserResponseDTO MapToDTO(User user)
        => new(
            user.Id,
            user.Name,
            user.Email,
            user.Loans.Select(loan => new UserLoanDTO(
                loan.Id,
                loan.Book.Title,
                loan.LoanDate,
                loan.DueDate,
                loan.IsActive
            )).ToList()
        );
}
