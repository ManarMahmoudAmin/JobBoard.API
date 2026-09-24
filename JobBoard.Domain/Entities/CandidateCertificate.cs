using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities
{
    public class CandidateCertificate
    {
        public int Id { get; set; }
        public string? CertificateName { get; set; }

        /*------------------------CandidateProfile--------------------------*/
        [ForeignKey("CandidateProfile")]
        public int CandidateProfileId { get; set; }
        public CandidateProfile CandidateProfile { get; set; }
    }
}
