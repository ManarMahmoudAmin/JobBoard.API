using JobBoard.Application.DTOs.CandidateApplicationDTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Features.CandidatApplications.Query.GetApplicationsByJobId
{
    public class GetApplicationsByJobIdQuery : IRequest<IEnumerable<RecruiterApplicationListDto>>
    {
        public int JobId { get; set; }
        public int RecruiterId { get; set; }
    }
}
