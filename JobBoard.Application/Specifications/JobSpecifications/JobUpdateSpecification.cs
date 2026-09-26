using JobBoard.Domain.Entities;

namespace JobBoard.Application.Specifications.JobSpecifications
{
    public class JobUpdateSpecification : BaseSpecifications<Job>
	{
		public JobUpdateSpecification(int id, int RecruiterId)
			: base(j => j.Id == id && j.RecruiterId == RecruiterId) // without IsApproved
		{
			AddIncludes(j => j.Skills);
			AddIncludes(j => j.Categories);
			AddIncludes(j => j.Recruiter);
		}
	}
}
