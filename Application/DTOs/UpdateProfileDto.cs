namespace Application.DTOs
{
    // Record ensures that both FirstName and LastName are provided.
    public record UpdateProfileDto(string FirstName, string LastName);
}
