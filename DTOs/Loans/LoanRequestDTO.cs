namespace LibraryAPI.DTOs.Loans;

public record LoanRequestDTO(
    int UserId,
    int BookId
);