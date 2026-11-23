namespace LibraryAPI.DTOs.Categories;

public record CategoryRequestDTO(
    string Name,
    string? Description
);