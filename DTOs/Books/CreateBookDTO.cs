namespace LibraryAPI.DTOs.Books;

public record CreateBookDTO(
    string Title,
    string ISBN,
    int PublicationYear,
    int AuthorId,
    int CategoryId
);