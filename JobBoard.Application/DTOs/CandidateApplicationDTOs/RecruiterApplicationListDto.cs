using JobBoard.Domain.Enums;

namespace JobBoard.Application.DTOs.CandidateApplicationDTOs

{
    public class RecruiterApplicationListDto
	{
		public int Id { get; set; }
		public string ApplicantName { get; set; }
		public string JobTitle { get; set; } 
		public string CurrentPosition { get; set; } 
		public string AppliedDate { get; set; }
		public string Experience { get; set; }
		public ApplicationStatus Status { get; set; }
		public string StatusDisplay { get; set; }
		public string ResumeUrl { get; set; }
	}
}
