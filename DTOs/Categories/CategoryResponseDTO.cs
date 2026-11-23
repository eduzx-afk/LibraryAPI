namespace LibraryAPI.DTOs.Categories;

public record CategoryResponseDTO(
    int Id,
    string Name,
    string? Description,
    List<CategoryBookDTO> Books
);

public record CategoryBookDTO(
    int Id,
    string Title,
    string ISBN,
    int PublicationYear,
    string AuthorName,
    bool IsAvailable
);