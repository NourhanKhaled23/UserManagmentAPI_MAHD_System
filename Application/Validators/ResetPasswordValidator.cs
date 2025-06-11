using FluentValidation;
using Application.DTOs;
namespace Application.Validators
{
   
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordValidator()
        {
            
            RuleFor(x => x.Email)
                .NotEmpty() 
                .EmailAddress() // Validates that the email has a correct format
                .WithMessage("Please provide a valid email address."); 

           
            RuleFor(x => x.Otp)
                .NotEmpty() // Ensures the OTP field is not empty
                .Length(6) // Checks that the OTP is exactly 6 characters
                .WithMessage("OTP must be exactly 6 digits."); 

           
            RuleFor(x => x.NewPassword)
                .NotEmpty() // Ensures the new password field is not empty
                .MinimumLength(6) // Validates that the password is at least 6 characters long
                .WithMessage("The new password must be at least 6 characters long."); 
        }
    }
}
