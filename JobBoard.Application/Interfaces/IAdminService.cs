using JobBoard.Application.DTOs;
using JobBoard.Application.DTOs.AdminDTOs;
using JobBoard.Application.DTOs.CandidateDTOs;
using JobBoard.Application.DTOs.JobDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Interfaces
{
    public interface IAdminService
    {
        Task<List<CandidateProfileDto>> GetAllCandidatesAsync();
        Task<List<RecruiterProfileDto>> GetAllRecruitersAsync();
        Task<CandidateProfileDto> GetCandidateByIdAsync(string id);
        Task<RecruiterProfileDto> GetRecruiterByIdAsync(string id);
        Task<List<JobDto>> GetAllJobsAsync();
        Task<bool> DeleteJob(int id);
        Task<List<JobDto>> GetPendingJobsAsync();
        Task<bool> ApproveJobAsync(int jobId);
        Task<bool> RejectJobAsync(int jobId);
        Task<bool> DeleteUserAsync(string userId);
        Task<StatsDto> GetStatsAsync();
    }
}
