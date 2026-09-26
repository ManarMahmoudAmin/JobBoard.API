using AutoMapper;
using JobBoard.Application.DTOs.RecruiterDTOs;
using JobBoard.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace JobBoard.Application.Mapping.Resolvers

{
    public class CompanyImageUrlResolver : IValueResolver<RecruiterSeedDto, RecruiterProfile, string>
	{
		private readonly IConfiguration _configuration; 

		public CompanyImageUrlResolver(IConfiguration configuration)
		{
			_configuration = configuration; 
		}

		public string Resolve(RecruiterSeedDto source, RecruiterProfile destination, string destMember, ResolutionContext context)
		{
			if (!string.IsNullOrEmpty(source.CompanyImage))
				return $"{_configuration["ApiBaseUrl"]}/{source.CompanyImage}";
			else
				return string.Empty;
		}
	}
}
