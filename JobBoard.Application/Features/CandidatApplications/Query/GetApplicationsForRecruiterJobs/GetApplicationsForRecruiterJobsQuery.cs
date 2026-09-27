using JobBoard.Application.DTOs.CandidateApplicationDTOs;
using JobBoard.Application.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Features.CandidatApplications.Query.GetApplicationsForRecruiterJobs
{
    public record GetApplicationsForRecruiterJobsQuery(int RecruiterId, ApplicationFilterParams filterParams) : 
        IRequest<IEnumerable<RecruiterApplicationListDto>>
    {
    }
}
