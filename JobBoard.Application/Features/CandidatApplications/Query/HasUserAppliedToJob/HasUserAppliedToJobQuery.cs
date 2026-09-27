using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Features.CandidatApplications.Query.HasUserAppliedToJob
{
    public class HasUserAppliedToJobQuery : IRequest<bool>
    {
        public int JobId { get; set; }
        public int ApplicantId { get; set; }
    }

}
