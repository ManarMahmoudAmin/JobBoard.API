using JobBoard.Application.Mapping.Profiles;
using JobBoard.Application.Mapping.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.API.Extensions
{
    public static class AutoMapperExtensions
    {
        public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<JobProfile>();
                cfg.AddProfile<RecruiterProfileMapping>();
                cfg.AddProfile<UserProfileMapping>();
                cfg.AddProfile<SkillAndCategoryProfile>();
                cfg.AddProfile<ApplicationProfile>();
                cfg.AddProfile<CandidateProfileMapping>();
                cfg.AddProfile<SavedJobProfile>();
                cfg.AddProfile<NotificationProfile>();
                //cfg.AddProfile<AdminProfile>();

            });

            services.AddScoped<CompanyImageUrlResolver>();
            services.AddScoped<CandidateCvUrlResolver>();
            services.AddScoped<ProfileImageUrlResolver>();
            services.AddScoped<ApplicationUrlResolver>();

            return services;
        }
    }
}