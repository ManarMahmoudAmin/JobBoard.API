namespace JobBoard.Application.DTOs.JobDTOs

{
    public class JobListDto
    {
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }

		// Recruiter Info (summary)
		public string CompanyName { get; set; } // From Recruiter
		public string Location { get; set; } //From Recruiter
		public decimal? Salary { get; set; }
		public string WorkplaceType { get; set; } // Enum
		public string JobType { get; set; } // Enum
		public DateTime PostedDate { get; set; }
		// List
		public List<string> Skills { get; set; } 
	}
}
