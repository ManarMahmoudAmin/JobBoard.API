using JobBoard.Domain.Enums;
using System.ComponentModel.DataAnnotations;

public class ExternalLoginReceiverDto : IValidatableObject
{
    public string IdToken { get; set; } = string.Empty;

    public string RoleFromClient { get; set; } = string.Empty;

    [StringLength(100, MinimumLength = 2)]
    public string? CompanyName { get; set; }

    [StringLength(200, MinimumLength = 3)]
    public string? CompanyLocation { get; set; }

    public IEnumerable<ValidationResult> Validate(
        ValidationContext validationContext)
    {
        if (RoleFromClient == UserType.Recruiter.ToString())
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