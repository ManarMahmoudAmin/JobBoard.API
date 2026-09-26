using JobBoard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.DTOs.CandidateDTOs.CandidateSeedDTOs
{
    public class CandidateSeedDto
    {
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Title { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? CV_Url { get; set; }
        public Gender? Gender { get; set; }
        public string? Summary { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? UserEmail { get; set; }
        public List<int>? Skills { get; set; }

        public List<CandidateCertificateSeedDto>? CandidateCertificates { get; set; }

        public List<CandidateTrainingSeedDto>? CandidateTraining { get; set; }

        public List<CandidateInterestsSeedDto> CandidateInterests { get; set; }
        public List<CandidateExperienceSeedDto>? CandidateExperiences { get; set; }

        public List<CandidateEducationSeedDto>? CandidateEducations { get; set; }
    }
}
