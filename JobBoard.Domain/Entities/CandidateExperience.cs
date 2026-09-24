using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities
{
    public class CandidateExperience
    {
        public int Id { get; set; }
        public string? JobTitle { get; set; }
        public string? CompanyName { get; set; }
        public string? Location { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Description { get; set; }

        /*------------------------CandidateProfile--------------------------*/

        [ForeignKey("CandidateProfile")]
        public int CandidateProfileId { get; set; }
        public CandidateProfile CandidateProfile { get; set; }


       
    }
}
