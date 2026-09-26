using JobBoard.Application.DTOs.AdminDTOs;
using JobBoard.Domain.Enums;

namespace JobBoard.Application.DTOs.AdminDTOs
{
	public class AdminUserDetailsDto
	{
		public string Id { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Email { get; set; }
		public string? UserName { get; set; }
		public string? PhoneNumber { get; set; }
		public UserType UserType { get; set; }
		public bool IsActive { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? LastLoginDate { get; set; }
		public AdminCandidateProfileDto? CandidateProfile { get; set; }
		public AdminRecruiterProfileDto? RecruiterProfile { get; set; }
		public string FullName => !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName)
			? $"{FirstName} {LastName}" : "N/A";
	}
}
