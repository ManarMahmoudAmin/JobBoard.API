namespace JobBoard.Application.DTOs.SavedJobDTOs
{
    public class SavedJobDto
    {
		public int Id { get; set; }
		public int JobId { get; set; }
		public string JobTitle { get; set; }
		public string Description { get; set; }
		public string CompanyName { get; set; }
		public string Location { get; set; } //From Recruiter
		public decimal? Salary { get; set; }
		public string WorkplaceType { get; set; }
		public string JobType { get; set; }
		public DateTime SavedAt { get; set; } = DateTime.UtcNow;
	}
}
