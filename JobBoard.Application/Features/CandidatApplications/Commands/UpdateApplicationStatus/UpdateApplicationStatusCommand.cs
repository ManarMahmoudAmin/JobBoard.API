using JobBoard.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Features.CandidatApplications.Commands.UpdateApplicationStatus
{
    public class UpdateApplicationStatusCommand : IRequest<bool>
    {
        public int ApplicationId { get; set; }
        public int RecruiterId { get; set; }
        public ApplicationStatus Status { get; set; }
    }
}
