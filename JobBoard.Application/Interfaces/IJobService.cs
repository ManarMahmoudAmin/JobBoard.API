using JobBoard.Application.DTOs.JobDTOs;
using JobBoard.Application.DTOs.SkillAndCategoryDTOs;
using JobBoard.Application.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Interfaces
{
    public interface IJobService
    {
        Task<IEnumerable<JobListDto>> GetAllJobsAsync(JobFilterParams filterParams);
        Task<IEnumerable<PostedJobsDto>> GetRecruiterJobsAsync(int RecruiterId, RecruiterJobFilterParams filterParams);
        Task<IEnumerable<TopPerformingJobDto>> GetTopPerformingJobsAsync(int RecruiterId, int limit = 5);
        Task<IEnumerable<RecentJobDto>> GetRecentJobsAsync(int RecruiterId, int limit = 3);
        Task<JobDto> GetJobByIdAsync(int id);
        Task<JobDto> AddJobAsync(CreateUpdateJobDto jobDto, int RecruiterId);
        Task<JobDto> UpdateJob(int id, CreateUpdateJobDto jobDto, int RecruiterId);
        Task<bool> DeleteJob(int id, int RecruiterId);
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<IEnumerable<SkillDto>> GetAllSkillsAsync();
    }
}
