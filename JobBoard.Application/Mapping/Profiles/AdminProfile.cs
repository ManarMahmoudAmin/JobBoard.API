using AutoMapper;
using JobBoard.Application.DTOs.AdminDTOs;
using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Mapping.Profiles
{
    internal class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<RecruiterProfile, RecruiterListDto>()
                .ForMember(dest => dest.Email, op => op.MapFrom(src => src.User.Email));


            CreateMap<CandidateProfile, CandidateListDto>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Skills.Select(s => s.SkillName)));
        }
    }
}
