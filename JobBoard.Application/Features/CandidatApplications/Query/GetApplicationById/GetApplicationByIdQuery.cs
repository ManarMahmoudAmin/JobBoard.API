using JobBoard.Application.DTOs.CandidateApplicationDTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Features.CandidatApplications.Query.GetApplicationById
{
    public class GetApplicationByIdQuery : IRequest<ApplicationDto>
    {
        public int Id { get; set; }
    }
}
