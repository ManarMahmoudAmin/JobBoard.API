using AutoMapper;
using JobBoard.Application.DTOs.SavedJobDTOs;
using JobBoard.Domain.Entities;


namespace JobBoard.Application.Mapping.Profiles
{
    public class SavedJobProfile : Profile
    {
		public SavedJobProfile()
		{
			CreateMap<SavedJob, SavedJobDto>()
				.ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.Job.Title))
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Job.Description))
				.ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Job.Recruiter.CompanyName))
				.ForMember(dest => dest.Location, op => op.MapFrom(src => src.Job.Recruiter.CompanyLocation))
				.ForMember(dest => dest.Salary, op => op.MapFrom(src => src.Job.Salary))
				.ForMember(dest => dest.WorkplaceType, op => op.MapFrom(src => src.Job.WorkplaceType))
				.ForMember(dest => dest.JobType, op => op.MapFrom(src => src.Job.JobType));

			CreateMap<CreateSavedJobDto, SavedJob>();
		}
	}
}
