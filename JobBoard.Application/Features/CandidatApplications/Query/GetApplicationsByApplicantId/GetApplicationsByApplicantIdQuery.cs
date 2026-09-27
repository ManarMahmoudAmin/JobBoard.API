using JobBoard.Application.DTOs.CandidateApplicationDTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Features.CandidatApplications.Query.GetApplicationsByApplicantId
{
    public class GetApplicationsByApplicantIdQuery :
        IRequest<IEnumerable<CandidateApplicationListDto>>
    {
        public int ApplicantId { get; set; }
    }
}
