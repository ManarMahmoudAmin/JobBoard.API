using JobBoard.Application.DTOs.CandidateApplicationDTOs;
using JobBoard.Application.Shared;
using JobBoard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Interfaces
{
    public interface IApplicationService
    {
        //CANDIDATE METHODS
        Task<ApplicationDto> CreateApplicationAsync(CreateApplicationDto createDto, int applicantId);
        Task<IEnumerable<CandidateApplicationListDto>> GetApplicationsByApplicantIdAsync(int applicantId);
        Task<bool> HasUserAppliedToJobAsync(int applicantId, int jobId);

        //RECRUITER METHODS
        Task<IEnumerable<RecruiterApplicationListDto>> GetApplicationsForRecruiterJobsAsync(int RecruiterId, ApplicationFilterParams filterParams);
        Task<IEnumerable<RecruiterApplicationListDto>> GetApplicationsByJobIdAsync(int jobId, int RecruiterId);
        Task<bool> UpdateApplicationStatusAsync(int applicationId, ApplicationStatus status, int RecruiterId);

        //ADMIN METHODS
        Task<bool> DeleteApplicationAsync(int id);
        //MUTUAL METHODS
        Task<ApplicationDto> GetApplicationByIdAsync(int id);

    }
}
