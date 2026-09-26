using AutoMapper;
using JobBoard.Application.DTOs.JobDTOs;
using JobBoard.Application.Shared;
using JobBoard.Domain.Entities;

namespace JobBoard.Application.Mapping.Profiles
{
    public class JobProfile : Profile
    {
        public JobProfile() {
            CreateMap<Job, JobListDto>()
				.ForMember(dest => dest.CompanyName, op => op.MapFrom(src => src.Recruiter.CompanyName))
				.ForMember(dest => dest.Location, op => op.MapFrom(src => src.Recruiter.CompanyLocation))
                .ForMember(dest => dest.Skills, op => op.MapFrom(src => src.Skills.Select(s => s.SkillName).ToList()));

            CreateMap<Job, JobDto>()
                .ForMember(dest => dest.CompanyName, op => op.MapFrom(src => src.Recruiter.CompanyName))
                .ForMember(dest => dest.CompanyLocation, op => op.MapFrom(src => src.Recruiter.CompanyLocation))
                .ForMember(dest => dest.CompanyImage, op => op.MapFrom(src => src.Recruiter.CompanyImage))
                .ForMember(dest => dest.Website, op => op.MapFrom(src => src.Recruiter.Website))
                .ForMember(dest => dest.Industry, op => op.MapFrom(src => src.Recruiter.Industry))
                .ForMember(dest => dest.CompanyDescription, op => op.MapFrom(src => src.Recruiter.CompanyDescription))
                .ForMember(dest => dest.CompanyMission, op => op.MapFrom(src => src.Recruiter.CompanyMission))
                .ForMember(dest => dest.EmployeeRange, op => op.MapFrom(src => src.Recruiter.EmployeeRange))
                .ForMember(dest => dest.EstablishedYear, op => op.MapFrom(src => src.Recruiter.EstablishedYear))
                .ForMember(dest => dest.Locaton, op => op.MapFrom(src => src.Recruiter.CompanyLocation))
                .ForMember(dest => dest.Email, op => op.MapFrom(src => src.Recruiter.User.Email))
                .ForMember(dest => dest.Phone, op => op.MapFrom(src => src.Recruiter.User.PhoneNumber))
                .ForMember(dest => dest.Categories, op => op.MapFrom(src => src.Categories.Select(c => c.CategoryName).ToList()))
                .ForMember(dest => dest.Skills, op => op.MapFrom(src => src.Skills.Select(s => s.SkillName).ToList()))
                .ReverseMap();

		CreateMap<JobSeedDto, Job>()
               .ForMember(dest => dest.Id, opt => opt.Ignore())
               .ForMember(dest => dest.RecruiterId, opt => opt.Ignore()) // Handle manually
               .ForMember(dest => dest.Recruiter, opt => opt.Ignore())
               .ForMember(dest => dest.Skills, opt => opt.Ignore()) // Handle manually
               .ForMember(dest => dest.Categories, opt => opt.Ignore()) // Handle manually
               .ForMember(dest => dest.JobApplications, opt => opt.Ignore());


            CreateMap<CreateUpdateJobDto, Job>()
               .ForMember(dest => dest.Id, opt => opt.Ignore())
               .ForMember(dest => dest.RecruiterId, opt => opt.Ignore())
               .ForMember(dest => dest.Categories, opt => opt.Ignore())
               .ForMember(dest => dest.Skills, opt => opt.Ignore());    
			   //.ForMember(dest => dest.PostedDate, opt => opt.Ignore());

			CreateMap<JobSeedDto, Job>();

            CreateMap<Job, TopPerformingJobDto>()
                .ForMember(dest => dest.ApplicationsCount, opt => opt.MapFrom(src => src.JobApplications != null ? src.JobApplications.Count : 0));

			CreateMap<Job, RecentJobDto>()
	            .ForMember(dest => dest.ApplicationsCount,
		            opt => opt.MapFrom(src => src.JobApplications != null ? src.JobApplications.Count : 0))
	            .ForMember(dest => dest.PostedAgo,
		            opt => opt.MapFrom(src => DateTimeHelper.CalculateTimeAgo(src.PostedDate)));

            CreateMap<Job, PostedJobsDto>()
                .ForMember(dest => dest.ApplicationsCount, opt => opt.MapFrom(src => src.JobApplications != null ? src.JobApplications.Count : 0));
		}
	}
    
}
