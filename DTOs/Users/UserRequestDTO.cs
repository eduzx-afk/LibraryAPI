namespace LibraryAPI.DTOs.Users;

public record UserRequestDTO(
    string Name,
    string? Email
);