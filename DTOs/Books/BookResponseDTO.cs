namespace LibraryAPI.DTOs.Books;

public record BookResponseDTO(
    int Id,
    string Title,
    string ISBN,
    int PublicationYear,
    string AuthorName,
    string CategoryName,
    bool IsAvailable
);