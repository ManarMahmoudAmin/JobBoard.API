using JobBoard.Application.Shared;
using JobBoard.Domain.Entities;
using JobBoard.Application.Specifications;

namespace JobBoard.Application.Specifications.ApplicationSpecifications

{
	public class ApplicationForRecruiterJobsSpecification : BaseSpecifications<CandidateApplication>
	{
		public ApplicationForRecruiterJobsSpecification(int RecruiterId, ApplicationFilterParams filterParams)
		   : base(a =>
			   a.Job.RecruiterId == RecruiterId &&
				  (string.IsNullOrWhiteSpace(filterParams.SearchValue) || a.Job.Title.ToLower().Contains(filterParams.SearchValue.ToLower())) &&
			   (!filterParams.JobId.HasValue || a.JobId == filterParams.JobId) &&
			   (!filterParams.ApplicantId.HasValue || a.ApplicantId == filterParams.ApplicantId) &&
			   (!filterParams.Status.HasValue || a.Status == filterParams.Status))
		{
			AddIncludes(a => a.Job);
			AddIncludes(a => a.Job.Recruiter);
			AddIncludes(a => a.Applicant);
			AddOrderByDesc(a => a.AppliedDate);

			//// Pagination
			//var skip = filterParams.PageSize * (filterParams.PageIndex - 1);
			//AddPagination(skip, filterParams.PageSize);
		}
	}
}