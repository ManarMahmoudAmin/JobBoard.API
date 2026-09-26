using AutoMapper;
using JobBoard.Application.DTOs.CandidateDTOs.CandidateSeedDTOs;
using JobBoard.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace JobBoard.Application.Mapping.Resolvers

{
    public class ProfileImageUrlResolver : IValueResolver<CandidateSeedDto, CandidateProfile, string>
    {
        private readonly IConfiguration _configuration;

        public ProfileImageUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(CandidateSeedDto source, CandidateProfile destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ProfileImageUrl))
                return $"{_configuration["ApiBaseUrl"]}/{source.ProfileImageUrl}";
            else
                return $"{_configuration["ApiBaseUrl"]}/images/profilepic/user.jpg";

        }
    }
}

