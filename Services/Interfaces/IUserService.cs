using LibraryAPI.Models;

namespace LibraryAPI.Services.Interfaces;

public interface UserService
{
    Task<List<User>> GetAllUsersAsync();

    Task<User?> GetUserByIdAsync(int id);

    Task<User> CreateUserAsync(
        string name,
        string? email
    );

    Task<bool> UpdateUserAsync(
        int id,
        string name,
        string? email
    );

    Task<bool> DeleteUserAsync(int id);

    Task<List<Loan>> GetUserLoansAsync(int id);
}
