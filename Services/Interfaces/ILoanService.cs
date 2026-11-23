using LibraryAPI.Models;

namespace LibraryAPI.Services.Interfaces;

public interface LoanService
{
    Task<Loan> BorrowBookAsync(int userId, int bookId);

    Task<bool> ReturnBookAsync(int loanId);

    Task<List<Loan>> GetActiveLoansAsync();

    Task<List<Loan>> GetLoanHistoryAsync(int userId);

    Task<Loan?> GetLoanByIdAsync(int id);
}
