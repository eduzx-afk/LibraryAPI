namespace LibraryAPI.DTOs.Loans;

public record LoanResponseDTO(
    int Id,
    string UserName,
    string BookTitle,
    string ISBN,
    DateTime LoanDate,
    DateTime DueDate,
    DateTime? ReturnDate,
    bool IsActive
);