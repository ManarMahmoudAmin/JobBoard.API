namespace JobBoard.Application.DTOs.JobDTOs

{
    public class TopPerformingJobDto
    {
		public int Id { get; set; }
		public string Title { get; set; }
		public int ApplicationsCount { get; set; }
		public DateTime PostedDate { get; set; }
	}
}
