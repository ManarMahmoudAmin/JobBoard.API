using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Features.CandidatApplications.Commands.DeleteApplication
{
    public record DeleteApplicationCommand(int id) : IRequest<bool>
    {
    }
}
