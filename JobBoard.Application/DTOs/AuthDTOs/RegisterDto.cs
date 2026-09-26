using JobBoard.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobBoard.Application.DTOs.AuthDTOs
{
    public class RegisterDto : IValidatableObject
    {
        [Required(ErrorMessage = "UserName is required")]
        [StringLength(50, ErrorMessage = "UserName must be between 3 and 50 characters",
            MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z0-9]+$",
            ErrorMessage = "UserName can only contain letters and numbers")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password must be at least 6 characters long",
            MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Confirm Password must be at least 6 characters long",
            MinimumLength = 6)]
        [Compare("Password", ErrorMessage = "Confirm Password does not match Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "User type is required")]
        [EnumDataType(typeof(UserType), ErrorMessage = "Invalid user type")]
        public UserType user_type { get; set; }

        /*------------------------Recruiter--------------------------*/

        [StringLength(100, ErrorMessage = "CompanyName must be between 3 and 100 characters",
            MinimumLength = 2)]
        public string? CompanyName { get; set; }

        [StringLength(200, ErrorMessage = "CompanyLocation must be between 3 and 200 characters",
            MinimumLength = 3)]
        public string? CompanyLocation { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (user_type == UserType.Recruiter)
            {
                if (string.IsNullOrWhiteSpace(CompanyName))
                {
                    yield return new ValidationResult(
                        "CompanyName is required for Recruiter",
                        new[] { nameof(CompanyName) });
                }

                if (string.IsNullOrWhiteSpace(CompanyLocation))
                {
                    yield return new ValidationResult(
                        "CompanyLocation is required for Recruiter",
                        new[] { nameof(CompanyLocation) });
                }
            }
        }
    }
}