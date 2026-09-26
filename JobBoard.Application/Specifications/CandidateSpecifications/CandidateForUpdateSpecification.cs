using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Specifications.CandidateSpecifications
{
    internal class CandidateForUpdateSpecification : BaseSpecifications<CandidateProfile>
    {
        public CandidateForUpdateSpecification(string userId) : base(x => x.UserId == userId)
        {
            AddIncludes(x => x.Skills);
            AddIncludes(x => x.CandidateInterests); 
            AddIncludes(x => x.CandidateCertificates);
            AddIncludes(x => x.CandidateTraining);
            AddIncludes(x => x.CandidateEducations);
            AddIncludes(x => x.CandidateExperiences); 
            AddIncludes(x => x.User);
        }
    }
}
