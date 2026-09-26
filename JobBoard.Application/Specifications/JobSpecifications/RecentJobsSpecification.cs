using JobBoard.Domain.Entities;

namespace JobBoard.Application.Specifications.JobSpecifications
{
    public class RecentJobsSpecification : BaseSpecifications<Job>
    {
		public RecentJobsSpecification(int RecruiterId, int limit = 3)
		: base(j => j.RecruiterId == RecruiterId && j.IsApproved)
		{
			AddIncludes(j => j.JobApplications);
			AddOrderByDesc(j => j.PostedDate);
			AddPagination(0, limit);
		}
	}
}
