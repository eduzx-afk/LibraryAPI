namespace LibraryAPI.DTOs.Users;

public record UserResponseDTO(
    int Id,
    string Name,
    string? Email,
    List<UserLoanDTO> Loans
);

public record UserLoanDTO(
    int Id,
    string BookTitle,
    DateTime LoanDate,
    DateTime DueDate,
    bool IsActive
);