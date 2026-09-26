using JobBoard.Domain.Enums;

namespace JobBoard.Application.DTOs.CandidateDTOs.CandidateSeedDTOs

{
    public class CandidateEducationSeedDto
    {
        public string? Major { get; set; }
        public string? Faculty { get; set; }
        public string? University { get; set; }
        public DateTime? Date { get; set; }
        public string? Location { get; set; }
        public double? GPA { get; set; }
        public EducationLevel? EducationLevel { get; set; }
    }
}
