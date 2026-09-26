using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Specifications.RecruiterSpecifications
{
    public class RecruiterSpecifications : BaseSpecifications<RecruiterProfile>
    {
        public RecruiterSpecifications() : base()
        {
            AddIncludes(r => r.User);
        }

        public RecruiterSpecifications(string userId) : base(x => x.UserId == userId) 
        {
            AddIncludes(r => r.User);
            AddIncludes(r => r.PostedJobs);
            AddIncludes(r => r.PostedJobs.Select(j => j.Skills)); }
    }
}
