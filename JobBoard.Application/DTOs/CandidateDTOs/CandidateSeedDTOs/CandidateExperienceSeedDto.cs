namespace JobBoard.Application.DTOs.CandidateDTOs.CandidateSeedDTOs
{
    public class CandidateExperienceSeedDto
    {
        public string? JobTitle { get; set; }
        public string? CompanyName { get; set; }
        public string? Location { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Description { get; set; }
        public List<int>? Skills { get; set; }
    }
}
