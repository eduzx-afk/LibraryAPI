using Microsoft.EntityFrameworkCore;
using LibraryAPI.Data;
using LibraryAPI.Models;
using LibraryAPI.Services.Interfaces;

namespace LibraryAPI.Services;

public class DefaultLoanService(LibraryDbContext context) : LoanService
{
    private readonly LibraryDbContext _context = context;

    public async Task<Loan> BorrowBookAsync(int userId, int bookId)
    {
        var user = await _context.Users
            .Include(u => u.Loans)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new KeyNotFoundException("Usuário não encontrado.");

        if (!user.CanBorrow)
            throw new InvalidOperationException(
                "O usuário atingiu o limite de empréstimos ativos."
            );

        var book = await _context.Books.FindAsync(bookId);

        if (book == null)
            throw new KeyNotFoundException("Livro não encontrado.");

        if (!book.IsAvailable)
            throw new InvalidOperationException(
                "O livro não está disponível."
            );

        book.Borrow();

        var loan = new Loan(user, book);

        _context.Loans.Add(loan);
        await _context.SaveChangesAsync();

        return loan;
    }

    public async Task<bool> ReturnBookAsync(int loanId)
    {
        var loan = await _context.Loans
            .Include(l => l.Book)
            .FirstOrDefaultAsync(
                l => l.Id == loanId
            );

        if (loan == null)
            return false;

        if (!loan.IsActive)
            throw new InvalidOperationException(
                "Este empréstimo já foi devolvido."
            );

        loan.Return();

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<Loan?> GetLoanByIdAsync(int id)
        => await _context.Loans
            .Include(l => l.Book)
            .Include(l => l.User)
            .FirstOrDefaultAsync(l => l.Id == id);

    public async Task<List<Loan>> GetActiveLoansAsync()
        => await _context.Loans
            .Include(l => l.Book)
            .Include(l => l.User)
            .Where(l => l.ReturnDate == null)
            .ToListAsync();

    public async Task<List<Loan>> GetLoanHistoryAsync(int userId)
        => await _context.Loans
            .Include(l => l.Book)
            .Include(l => l.User)
            .Where(l => l.UserId == userId)
            .ToListAsync();
}
