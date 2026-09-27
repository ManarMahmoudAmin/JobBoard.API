using JobBoard.Application.DTOs.CandidateDTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Features.Candidate.Query.GetByUserId
{
    public record GetByUserIdQuery(string userId) : IRequest<CandidateProfileDto?>
    {
    }
}
