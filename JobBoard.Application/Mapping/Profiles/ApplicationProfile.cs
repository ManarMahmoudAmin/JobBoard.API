using AutoMapper;
using JobBoard.Application.DTOs.CandidateApplicationDTOs;
using JobBoard.Application.Mapping.Resolvers;
using JobBoard.Application.Shared;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Enums;

namespace JobBoard.Application.Mapping.Profiles

{
    public class ApplicationProfile : Profile
	{
		public ApplicationProfile()
		{
			CreateMap<CreateApplicationDto, CandidateApplication>()
			.ForMember(dest => dest.AppliedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
			.ForMember(dest => dest.Status, opt => opt.MapFrom(src => ApplicationStatus.Pending))
			.ForMember(dest => dest.ResumeUrl, opt => opt.Ignore());

			CreateMap<CandidateApplication, ApplicationDto>()
				.ForMember(dest => dest.Job, opt => opt.MapFrom(src => src.Job));

			CreateMap<Job, JobSummaryDto>()
				.ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Recruiter != null ? src.Recruiter.CompanyName : string.Empty))
				.ForMember(dest => dest.CompanyLocation, opt => opt.MapFrom(src => src.Recruiter.CompanyLocation))
				.ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.Salary));

			CreateMap<CandidateProfile, ApplicantSummaryDto>();


			CreateMap<(ApplicationSeedDto userDto, ApplicationDetailDto appDto), CandidateApplication>()
			   .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.userDto.FullName))
			   .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.userDto.Email))
			   .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.userDto.PhoneNumber))
			   .ForMember(dest => dest.CurrentLocation, opt => opt.MapFrom(src => src.userDto.CurrentLocation))
			   .ForMember(dest => dest.CurrentJobTitle, opt => opt.MapFrom(src => src.userDto.CurrentJobTitle))
			   .ForMember(dest => dest.YearsOfExperience, opt => opt.MapFrom(src => src.userDto.YearsOfExperience))
			   .ForMember(dest => dest.ResumeUrl, opt => opt.MapFrom<ApplicationUrlResolver>())
			   .ForMember(dest => dest.PortfolioUrl, opt => opt.MapFrom(src => src.userDto.PortfolioUrl))
			   .ForMember(dest => dest.LinkedInUrl, opt => opt.MapFrom(src => src.userDto.LinkedInUrl))
			   .ForMember(dest => dest.GitHubUrl, opt => opt.MapFrom(src => src.userDto.GitHubUrl))
			   .ForMember(dest => dest.CoverLetter, opt => opt.MapFrom(src => src.appDto.CoverLetter))
			   .ForMember(dest => dest.AppliedDate, opt => opt.MapFrom(src => src.appDto.AppliedDate))
			   .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.appDto.Status))
			   .ForMember(dest => dest.ApplicantId, opt => opt.MapFrom(src => src.userDto.ApplicantId))
			   .ForMember(dest => dest.JobId, opt => opt.MapFrom(src => src.appDto.JobId))
			   .ForMember(dest => dest.Id, opt => opt.Ignore())
			   .ForMember(dest => dest.Job, opt => opt.Ignore())
			   .ForMember(dest => dest.Applicant, opt => opt.Ignore());

			CreateMap<CandidateApplication, RecruiterApplicationListDto>()
		   .ForMember(dest => dest.ApplicantName, opt => opt.MapFrom(src => src.FullName))
		   .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.Job.Title))
		   .ForMember(dest => dest.CurrentPosition, opt => opt.MapFrom(src => src.CurrentJobTitle))
		   .ForMember(dest => dest.AppliedDate, opt => opt.MapFrom(src => DateTimeHelper.CalculateTimeAgo(src.AppliedDate)))
		   .ForMember(dest => dest.Experience, opt => opt.MapFrom(src => $"{src.YearsOfExperience} years experience"))
		   .ForMember(dest => dest.StatusDisplay, opt => opt.MapFrom(src => ApplicationStatusHelper.GetStatusDisplay(src.Status)));

			CreateMap<CandidateApplication, CandidateApplicationListDto>()
		   .ForMember(dest => dest.ApplicationId, opt => opt.MapFrom(src => src.Id))
		   .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.Job.Title))
		   .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Job.Recruiter.CompanyName))
		   .ForMember(dest => dest.CompanyLocation, opt => opt.MapFrom(src => src.Job.Recruiter.CompanyLocation))
		   .ForMember(dest => dest.CompanyImage, opt => opt.MapFrom(src => src.Job.Recruiter.CompanyImage))
		   .ForMember(dest => dest.AppliedDate, opt => opt.MapFrom(src => DateTimeHelper.CalculateTimeAgo(src.AppliedDate)))
		   .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

		}
	}
}