namespace Application.DTOs
{
    // Record ensures that OldPassword and NewPassword are provided.
    public record ChangePasswordDto(string OldPassword, string NewPassword);
}
