using AutoMapper;
using JobBoard.Application.DTOs.CandidateDTOs.CandidateSeedDTOs;
using JobBoard.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace JobBoard.Application.Mapping.Resolvers
{
    public class CandidateCvUrlResolver : IValueResolver<CandidateSeedDto, CandidateProfile, string>
	{
        private readonly IConfiguration _configuration;

		public CandidateCvUrlResolver(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public string Resolve(CandidateSeedDto source, CandidateProfile destination, string destMember, ResolutionContext context)
		{
			if(!string.IsNullOrEmpty(source.CV_Url))
				return $"{_configuration["ApiBaseUrl"]}/{source.CV_Url}";
			else
				return string.Empty;
		}
	}
}
