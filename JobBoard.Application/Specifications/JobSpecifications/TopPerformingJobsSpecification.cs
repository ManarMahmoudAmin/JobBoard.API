using JobBoard.Domain.Entities;

namespace JobBoard.Application.Specifications.JobSpecifications

{
    public class TopPerformingJobsSpecification : BaseSpecifications<Job>
	{
		public TopPerformingJobsSpecification(int RecruiterId, int limit = 5)
			: base(j => j.RecruiterId == RecruiterId &&
					   j.IsApproved)
		{
			AddIncludes(j => j.JobApplications);
			AddOrderByDesc(j => j.JobApplications.Count);
			AddPagination(0, limit);
		}
	}
}
