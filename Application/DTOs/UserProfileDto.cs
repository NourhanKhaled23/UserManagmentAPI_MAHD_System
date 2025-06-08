namespace Application.DTOs
{
   
        public record UserProfileDto(
        int Id,
        string FirstName,
        string LastName,
        string Email,
        string Role
    );
    //Using a record ensures that once a UserProfileDto is created, its properties cannot be changed.
}
