using AutoMapper;
using JobBoard.Application.DTOs;
using JobBoard.Application.DTOs.RecruiterDTOs;
using JobBoard.Domain.Entities;

namespace JobBoard.Application.Mapping.Profiles

{
    public class RecruiterProfileMapping : Profile
    {
        public RecruiterProfileMapping()
        {
            CreateMap<RecruiterProfile, RecruiterProfileDto>()
                .ForMember(dest=> dest.Email , op=> op.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Phone, op => op.MapFrom(src => src.User.PhoneNumber))
                .ReverseMap();

            CreateMap<RecruiterProfileUpdateDto, RecruiterProfile>()
                .ForMember(dest => dest.CompanyImage, opt => opt.Ignore());


		}
	}
}
