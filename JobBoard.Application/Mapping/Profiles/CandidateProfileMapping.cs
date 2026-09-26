using AutoMapper;
using JobBoard.Application.DTOs.CandidateDTOs;
using JobBoard.Application.DTOs.CandidateDTOs.CandidateSeedDTOs;
using JobBoard.Application.Mapping.Resolvers;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Enums;

namespace JobBoard.Application.Mapping.Profiles
{


    public class CandidateProfileMapping : Profile
	{
		public CandidateProfileMapping()
		{
			// -----------------------------
			// Candidate Profile -> DTO
			// -----------------------------
			CreateMap<CandidateProfile, CandidateProfileDto>()
				.ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
				.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.UserName))
				.ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Skills.Select(s => s.SkillName)))
				.ForMember(dest => dest.InterestName, opt => opt.MapFrom(src => src.CandidateInterests.Select(i => i.InterestName)))
				.ForMember(dest => dest.CertificateName, opt => opt.MapFrom(src => src.CandidateCertificates.Select(c => c.CertificateName)))
				.ForMember(dest => dest.TrainingName, opt => opt.MapFrom(src => src.CandidateTraining.Select(t => t.TrainingName)))
				.ForMember(dest => dest.CandidateEducations, opt => opt.MapFrom(src => src.CandidateEducations))
				.ForMember(dest => dest.CandidateExperiences, opt => opt.MapFrom(src => src.CandidateExperiences));

			// -----------------------------
			// DTO -> Candidate Profile
			// -----------------------------
			CreateMap<CandidateProfileUpdateDto, CandidateProfile>()
				.ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.Skills.Select(s => new Skill { SkillName = s })))
				.ForMember(dest => dest.CandidateInterests, opt => opt.MapFrom(src => src.Interests.Select(i => new CandidateInterest { InterestName = i })))
				.ForMember(dest => dest.CandidateCertificates, opt => opt.MapFrom(src => src.Certificates.Select(c => new CandidateCertificate { CertificateName = c })))
				.ForMember(dest => dest.CandidateTraining, opt => opt.MapFrom(src => src.Trainings.Select(t => new CandidateTraining { TrainingName = t })))
				.ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));


			//------------------------------
			// Candidate Profile Seed
			//------------------------------
			CreateMap<CandidateSeedDto, CandidateProfile>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.UserId, opt => opt.Ignore())
				.ForMember(dest => dest.User, opt => opt.Ignore())
				.ForMember(dest => dest.UserApplications, opt => opt.Ignore())

				// Handle nullable Gender properly
				.ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender ?? Gender.Male))

				// Use resolvers only if they exist, otherwise direct mapping
				.ForMember(dest => dest.CV_Url, opt => opt.MapFrom<CandidateCvUrlResolver>())
				.ForMember(dest => dest.ProfileImageUrl, opt => opt.MapFrom<ProfileImageUrlResolver>())
				//.ForMember(dest => dest.CV_Url, opt => opt.MapFrom(src => src.CV_Url))
				//.ForMember(dest => dest.ProfileImageUrl, opt => opt.MapFrom(src => src.ProfileImageUrl))

				// Collections - will be handled manually in seeder
				.ForMember(dest => dest.Skills, opt => opt.Ignore())
				.ForMember(dest => dest.CandidateCertificates, opt => opt.Ignore())
				.ForMember(dest => dest.CandidateTraining, opt => opt.Ignore())
				.ForMember(dest => dest.CandidateInterests, opt => opt.Ignore())
				.ForMember(dest => dest.CandidateExperiences, opt => opt.Ignore())
				.ForMember(dest => dest.CandidateEducations, opt => opt.Ignore());


            // -----------------------------
            // Education
            // -----------------------------
            CreateMap<CandidateEducation, CandidateEducationDto>().ReverseMap();


			CreateMap<CandidateEducationUpdateDto, CandidateEducation>()
				.ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));


			CreateMap<CandidateEducationSeedDto, CandidateEducation>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.CandidateProfileId, opt => opt.Ignore())
				.ForMember(dest => dest.CandidateProfile, opt => opt.Ignore())
				.ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));



            // -----------------------------
            // Experience
            // -----------------------------
            CreateMap<CandidateExperience, CandidateExperienceDto>().ReverseMap();


			CreateMap<CandidateExperienceUpdateDto, CandidateExperience>()
				.ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

			CreateMap<CandidateExperienceSeedDto, CandidateExperience>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.CandidateProfileId, opt => opt.Ignore())
				.ForMember(dest => dest.CandidateProfile, opt => opt.Ignore())
				.ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));


			// -----------------------------
			// Candidate Intersts Seed
			// -----------------------------
			CreateMap<CandidateInterestsSeedDto, CandidateInterest>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.CandidateProfileId, opt => opt.Ignore())
				.ForMember(dest => dest.CandidateProfile, opt => opt.Ignore());
			//.ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

			// -----------------------------
			// Candidate Training Seed
			// -----------------------------
			CreateMap<CandidateTrainingSeedDto, CandidateTraining>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.CandidateProfileId, opt => opt.Ignore())
				.ForMember(dest => dest.CandidateProfile, opt => opt.Ignore());
			//.ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));


			// -----------------------------
			// Candidate Certificate Seed
			// -----------------------------
			CreateMap<CandidateCertificateSeedDto, CandidateCertificate>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.CandidateProfileId, opt => opt.Ignore())
				.ForMember(dest => dest.CandidateProfile, opt => opt.Ignore());
            //.ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));



        }





    }
    }
