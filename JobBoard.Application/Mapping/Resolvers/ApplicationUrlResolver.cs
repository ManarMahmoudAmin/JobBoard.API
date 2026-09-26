using AutoMapper;
using JobBoard.Application.DTOs.CandidateApplicationDTOs;
using JobBoard.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Mapping.Resolvers
{
    public class ApplicationUrlResolver : IValueResolver<(ApplicationSeedDto userDto, ApplicationDetailDto appDto), CandidateApplication, string>
    {
        private readonly IConfiguration _configuration;

        public ApplicationUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        string IValueResolver<(ApplicationSeedDto userDto, ApplicationDetailDto appDto), CandidateApplication, string>.Resolve(
            (ApplicationSeedDto userDto, ApplicationDetailDto appDto) source,
            CandidateApplication destination,
            string destMember,
            ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.userDto.ResumeUrl))
                return $"{_configuration["ApiBaseUrl"]}/{source.userDto.ResumeUrl}";
            else
                return string.Empty;
        }
    }
}
