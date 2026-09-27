using JobBoard.Application.Attributes;
using JobBoard.Application.DTOs.CandidateApplicationDTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JobBoard.Application.Features.CandidatApplications.Commands.CreateApplication
{
    public record CreateApplicationCommand(CreateApplicationDto CreateDto, int ApplicantId) 
        : IRequest<ApplicationDto>;
}
