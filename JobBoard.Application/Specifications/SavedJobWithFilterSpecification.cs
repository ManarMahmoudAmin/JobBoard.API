using JobBoard.Application.Shared;
using JobBoard.Application.Shared.SortingOptions;
using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Specifications
{
    public class SavedJobWithFilterSpecification : BaseSpecifications<SavedJob>
    {
        public SavedJobWithFilterSpecification(int CandidateId, SavedJobFilterParams filterParams)
        : base(s =>
            s.CandidateId == CandidateId &&
            (string.IsNullOrEmpty(filterParams.SearchValue) ||
            (
                s.Job.Title.ToLower().Contains(filterParams.SearchValue.ToLower()) ||
                s.Job.Recruiter.CompanyName.ToLower().Contains(filterParams.SearchValue.ToLower()) ||
                s.Job.Recruiter.CompanyLocation.ToLower().Contains(filterParams.SearchValue.ToLower())
            ))
        )
        {
            AddIncludes(s => s.Job);
            AddIncludes(s => s.Job.Recruiter);

            switch (filterParams.SortingOption)
            {
                case SortingDateOptions.DateAsc:
                    AddOrderBy(s => s.SavedAt);
                    break;
                default:
                    AddOrderByDesc(s => s.SavedAt);
                    break;
            }
        }

        public SavedJobWithFilterSpecification(int CandidateId, int jobId)
            : base(s => s.CandidateId == CandidateId && s.JobId == jobId)
        {
            AddIncludes(s => s.Job);
            AddIncludes(s => s.Job.Recruiter);
        }

        public SavedJobWithFilterSpecification(int savedJobId)
            : base(s => s.Id == savedJobId)
        {
            AddIncludes(s => s.Job);
            AddIncludes(s => s.Job.Recruiter);
        }
    }
}
