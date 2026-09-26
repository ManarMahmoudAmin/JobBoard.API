namespace JobBoard.Application.DTOs.CandidateApplicationDTOs

{
    public class JobSummaryDto
    {
		public int Id { get; set; }
		public string Title { get; set; }
		public string CompanyName { get; set; }
		public string CompanyLocation { get; set; }
		public decimal? Salary { get; set; }
		public  int RecruiterId { get; set; }
	}
}
