namespace LibraryAPI.DTOs.Authors;

public record AuthorResponseDTO(
    int Id,
    string Name,
    string? Biography,
    List<AuthorBookDTO> Books
);

public record AuthorBookDTO(
    int Id,
    string Title,
    string ISBN,
    int PublicationYear,
    string CategoryName,
    bool IsAvailable
);