namespace JobBoard.Application.DTOs.AdminDTOs
{
    public class AdminCandidateProfileDto : RecruiterProfileDto
	{
		public bool IsActive { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string? UserName { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string FullName => !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName)
			? $"{FirstName} {LastName}" : "N/A";
		public int JobsPostedCount { get; set; }
		public int ActiveJobsCount { get; set; }
		public DateTime? LastLoginDate { get; set; }
		public string StatusDisplay => IsActive ? "Active" : "Inactive";
	}
}
