using JobBoard.Application.DTOs.CandidateDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Interfaces
{
    public interface ICandidateService
    {
        Task<IEnumerable<CandidateProfileDto>> GetAllAsync();
        Task<CandidateProfileDto> GetByUserIdAsync(string userId);
        Task<bool> UpdateAsync(string userId, CandidateProfileUpdateDto model);
        Task<(string? CvUrl, string? ProfileImageUrl)> UploadFilesAsync(string userId, CandidateFileUploadDto dto);
        Task<bool> DeleteAsync(int Id);
    }
}
