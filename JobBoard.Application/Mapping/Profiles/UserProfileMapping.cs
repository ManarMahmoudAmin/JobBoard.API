using AutoMapper;
using JobBoard.Application.DTOs;
using JobBoard.Application.DTOs.RecruiterDTOs;
using JobBoard.Application.Mapping.Resolvers;
using JobBoard.Domain.Entities;
namespace JobBoard.Application.Mapping.Profiles

{
    public class UserProfileMapping :Profile
    {
        public UserProfileMapping()
        {
            // DTO to Entity mappings
            CreateMap<UserSeedDto, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());

            CreateMap<RecruiterSeedDto, RecruiterProfile>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.CompanyImage, opt => opt.MapFrom<CompanyImageUrlResolver>());



        }
    }
}
