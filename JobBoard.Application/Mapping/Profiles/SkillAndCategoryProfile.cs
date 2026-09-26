using AutoMapper;
using JobBoard.Application.DTOs.SkillAndCategoryDTOs;
using JobBoard.Domain.Entities;

namespace JobBoard.Application.Mapping.Profiles
{
    public class SkillAndCategoryProfile : Profile
    {
		public SkillAndCategoryProfile()
		{
			CreateMap<Category, CategoryDto>();
			CreateMap<Skill, SkillDto>();
		}
	}
}
