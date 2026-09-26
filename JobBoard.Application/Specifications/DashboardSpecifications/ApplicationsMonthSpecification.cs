using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Specifications.DashboardSpecifications
{
    public class ApplicationsMonthSpecification : BaseSpecifications<CandidateApplication>
    {
        public ApplicationsMonthSpecification(int RecruiterId, DateTime startOfMonth)
            : base(a => a.Job.RecruiterId == RecruiterId && a.AppliedDate >= startOfMonth)
        {
            AddIncludes(a => a.Job);
        }
    }
}