using JobBoard.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Features.Recruiter.Query.GetByUserIdForRecruiter
{
    public class GetByUserIdForRecruiterQuery : IRequest<RecruiterProfileDto?>
    {
        public string UserId { get; set; }
    }
}
