using JobBoard.Domain.Enums;

namespace JobBoard.Application.DTOs.CandidateApplicationDTOs

{
    public class ApplicationDetailDto
	{
		public string? CoverLetter { get; set; }
		public DateTime AppliedDate { get; set; }
		public ApplicationStatus Status { get; set; }
		public int JobId { get; set; }
	}
}
