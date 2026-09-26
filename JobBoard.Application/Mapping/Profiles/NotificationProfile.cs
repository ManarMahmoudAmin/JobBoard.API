using AutoMapper;
using JobBoard.Application.DTOs.NotificationDTOs;
using JobBoard.Domain.Entities;

namespace JobBoard.Application.Mapping.Profiles

{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationDto>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
             .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message))
             .ForMember(dest => dest.IsRead, opt => opt.MapFrom(src => src.IsRead))
             .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt)) // map صريح لـ CreatedAt
             .ForMember(dest => dest.Link, opt => opt.MapFrom(src => src.Link ?? string.Empty));

            CreateMap<NotificationDto, Notification>()
                .ReverseMap();
        }
    }
    
}
