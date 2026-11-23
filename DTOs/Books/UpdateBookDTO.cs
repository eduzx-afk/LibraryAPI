namespace LibraryAPI.DTOs.Books;

public record UpdateBookDTO(
    string Title,
    int PublicationYear
);