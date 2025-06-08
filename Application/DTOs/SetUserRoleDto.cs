
namespace Application.DTOs
{
    // Record ensures that Role is provided during object creation.
    public record SetUserRoleDto(string Role);// e.g., "Admin", "Instructor", "Student"
}
