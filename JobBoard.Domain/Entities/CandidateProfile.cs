using JobBoard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities
{
    public class CandidateProfile 
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? Title { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address {  get; set; }
        public string? CV_Url { get; set; }
        public Gender Gender { get; set; }
        public string? Summary { get; set; }
        public string? ProfileImageUrl { get; set; }



        /*------------------------user--------------------------*/
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        /*------------------------Application--------------------------*/
        public List<Application>? UserApplications { get; set; }

		/*------------------------Skills--------------------------*/
		public ICollection<Skill>? Skills { get; set; } = new List<Skill>();

        /*------------------------CandidateEducation--------------------------*/
        public ICollection<CandidateEducation>? CandidateEducations { get; set; } = new List<CandidateEducation>();

        /*------------------------CandidateExperience--------------------------*/
        public ICollection<CandidateExperience>? CandidateExperiences { get; set; } = new List<CandidateExperience>();


        /*------------------------CandidateTraining--------------------------*/
        public ICollection<CandidateTraining>? CandidateTraining { get; set; } = new List<CandidateTraining>();


        /*------------------------CandidateCertificate--------------------------*/
        public ICollection<CandidateCertificate>? CandidateCertificates { get; set; } = new List<CandidateCertificate>();


        /*------------------------CandidateInterest--------------------------*/
        public ICollection<CandidateInterest>? CandidateInterests { get; set; } = new List<CandidateInterest>();

    }
}
