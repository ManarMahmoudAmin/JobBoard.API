using JobBoard.Domain.Entities;
using JobBoard.Application.Specifications;

namespace JobBoard.Application.Specifications.JobSpecifications
{
    public class JobsWithDetailsSpec : BaseSpecifications<Job>
    {
        public JobsWithDetailsSpec():base(j => j.IsApproved) {
            AddIncludes(j => j.Recruiter);
            AddIncludes(j => j.Skills);
            AddIncludes(j => j.Categories);
        }
    }
}
