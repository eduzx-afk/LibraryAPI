namespace LibraryAPI.DTOs.Authors;

public record AuthorRequestDTO(
    string Name,
    string? Biography
);