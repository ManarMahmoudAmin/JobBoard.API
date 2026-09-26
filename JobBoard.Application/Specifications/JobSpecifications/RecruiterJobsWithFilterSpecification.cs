using JobBoard.Application.Shared;
using JobBoard.Application.Shared.SortingOptions;
using JobBoard.Domain.Entities;

namespace JobBoard.Application.Specifications.JobSpecifications

{
    public class RecruiterJobsWithFilterSpecification : BaseSpecifications<Job>
	{
		public RecruiterJobsWithFilterSpecification(int RecruiterId, RecruiterJobFilterParams filterParams)
			: base(j => j.RecruiterId == RecruiterId &&
					   j.IsApproved &&
					   // Search by title
					   (string.IsNullOrWhiteSpace(filterParams.SearchValue) ||
						j.Title.ToLower().Contains(filterParams.SearchValue.ToLower())) &&
					   // Status filter
					   (!filterParams.Status.HasValue ||
						(filterParams.Status == RecruiterJobStatus.Active &&
						 j.IsActive &&
						 (!j.ExpireDate.HasValue || j.ExpireDate > DateTime.Now)) ||
						(filterParams.Status == RecruiterJobStatus.Filled && !j.IsActive) ||
						(filterParams.Status == RecruiterJobStatus.Expired &&
						 j.ExpireDate.HasValue &&
						 j.ExpireDate < DateTime.Now)))
		{
			AddIncludes(j => j.JobApplications);
			AddIncludes(j => j.Recruiter);

			// Sorting
			switch (filterParams.SortingOption)
			{
				case RecruiterJobSortingOptions.ApplicationsCountDesc:
					AddOrderByDesc(j => j.JobApplications.Count);
					break;
				case RecruiterJobSortingOptions.PostedDateAsc:
					AddOrderBy(j => j.PostedDate);
					break;
				default: 
					AddOrderByDesc(j => j.PostedDate);
					break;
			}
		}
	}
}