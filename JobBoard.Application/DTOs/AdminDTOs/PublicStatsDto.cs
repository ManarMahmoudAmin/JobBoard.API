namespace JobBoard.Application.DTOs.AdminDTOs
{
    public class PublicStatsDto
	{
		public int TotalCandidates { get; set; }
		public int TotalRecruiters { get; set; }
		public int TotalJobs { get; set; }
		public int ApprovedJobs { get; set; }
		public int ActiveJobs { get; set; }
		public double JobsGrowth { get; set; }
		public double ApprovalGrowth { get; set; }
		public double ActiveJobsGrowth { get; set; }
	}
}
