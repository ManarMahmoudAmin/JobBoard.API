using JobBoard.Application.DTOs.SavedJobDTOs;
using JobBoard.Application.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Interfaces
{
    public interface ISavedJobService
    {
        Task<IEnumerable<SavedJobDto>> GetSavedJobsAsync(int CandidateId, SavedJobFilterParams filterParams);
        Task<SavedJobDto> GetSavedJobByIdAsync(int CandidateId, int jobId);
        Task<SavedJobDto> SaveJobAsync(int CandidateId, CreateSavedJobDto createSavedJobDto);
        Task<bool> UnsaveJobAsync(int CandidateId, int jobId);
        Task<bool> IsJobSavedAsync(int CandidateId, int jobId);
    }
}
