using Microsoft.EntityFrameworkCore;
using LibraryAPI.Data;
using LibraryAPI.Models;
using LibraryAPI.Services.Interfaces;

namespace LibraryAPI.Services;

public class DefaultUserService(LibraryDbContext context) : UserService
{
    private readonly LibraryDbContext _context = context;

    public async Task<List<User>> GetAllUsersAsync()
        => await _context.Users
            .Include(u => u.Loans)
                .ThenInclude(l => l.Book)
            .ToListAsync();

    public async Task<User?> GetUserByIdAsync(int id)
        => await _context.Users
            .Include(u => u.Loans)
                .ThenInclude(l => l.Book)
            .FirstOrDefaultAsync(u => u.Id == id);

    public async Task<User> CreateUserAsync(
        string name,
        string? email)
    {
        var normalizedName = Normalize(name);

        bool nameExists = await _context.Users.AnyAsync(
            u => u.Name.Trim().ToLower() == normalizedName
        );

        if (nameExists)
            throw new ArgumentException("Nome já existe.");

        if (!string.IsNullOrWhiteSpace(email))
        {
            var normalizedEmail = Normalize(email);

            bool emailExists = await _context.Users.AnyAsync(
                u => u.Email != null &&
                     u.Email.Trim().ToLower() == normalizedEmail
            );

            if (emailExists)
                throw new ArgumentException("Email já cadastrado.");
        }

        var user = new User(
            name,
            email
        );

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<bool> UpdateUserAsync(
        int id,
        string name,
        string? email)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return false;

        var normalizedName = Normalize(name);

        bool nameExists = await _context.Users.AnyAsync(
            u => u.Id != id &&
                 u.Name.Trim().ToLower() == normalizedName
        );

        if (nameExists)
            throw new ArgumentException("Nome já existe.");

        if (!string.IsNullOrWhiteSpace(email))
        {
            var normalizedEmail = Normalize(email);

            bool emailExists = await _context.Users.AnyAsync(
                u => u.Id != id &&
                     u.Email != null &&
                     u.Email.Trim().ToLower() == normalizedEmail
            );

            if (emailExists)
                throw new ArgumentException("Email já cadastrado.");
        }

        user.UpdateName(name);
        user.UpdateEmail(email);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return false;

        bool hasLoans = await _context.Loans
            .AnyAsync(l => l.UserId == id);

        if (hasLoans)
            throw new InvalidOperationException(
                "Não é possível excluir um usuário que possui histórico de empréstimos."
            );

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<Loan>> GetUserLoansAsync(int id)
        => await _context.Loans
            .Include(l => l.Book)
            .Where(l => l.UserId == id)
            .ToListAsync();

    private static string Normalize(string value)
        => value.Trim().ToLowerInvariant();
}
