using AutoMapper;
using JobBoard.Application.DTOs.CandidateApplicationDTOs;
using JobBoard.Application.Interfaces;
using JobBoard.Application.Shared;
using JobBoard.Application.Specifications.ApplicationSpecifications;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Enums;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly INotificationService _notificationService;
        private readonly IDocumentService _documentService;


        public ApplicationService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IConfiguration configuration,
            INotificationService notificationService,
            IDocumentService documentService)

        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _notificationService = notificationService;
            _documentService = documentService;
        }

        // ---------------------Candidate METHODS--------------------
        /// Create an application for a Candidate
        public async Task<ApplicationDto> CreateApplicationAsync(CreateApplicationDto createDto, int applicantId)
        {
            //Verify if user has already applied to this job
            var hasApplied = await HasUserAppliedToJobAsync(applicantId, createDto.JobId);
            if (hasApplied)
                return null;


            // Get the job details to access Recruiter information
            var spec = new JobFinderSpecification(createDto.JobId);
            var job = await _unitOfWork.Repository<Job>().GetByIdAsync(spec);
            if (job == null)
                return null;

            var application = _mapper.Map<CandidateApplication>(createDto);
            application.ApplicantId = applicantId;
            application.AppliedDate = DateTime.UtcNow;
            application.Status = ApplicationStatus.Pending;

            if (createDto.ResumeUrl != null && createDto.ResumeUrl.Length > 0)
            {
                application.ResumeUrl = await _documentService.UploadFileAsync(createDto.ResumeUrl, "cv");
            }

            await _unitOfWork.Repository<CandidateApplication>().AddAsync(application);

            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0)
                return null;

            // Send notification to Recruiter after successful save
            await SendApplicationNotificationAsync(job, createDto.FullName, application.Id);

            return _mapper.Map<ApplicationDto>(application);

        }

        /// Gets all applications of a specific Candidate
        public async Task<IEnumerable<CandidateApplicationListDto>> GetApplicationsByApplicantIdAsync(int applicantId)
        {
            var spec = new ApplicationWithFilterSpecification(applicantId, isApplicantId: true);
            var applications = await _unitOfWork.Repository<CandidateApplication>().GetAllAsync(spec);
            var mappedApplications = _mapper.Map<IEnumerable<CandidateApplicationListDto>>(applications);
            return mappedApplications;
        }

        /// Checks if a Candidate has already applied to a specific job
        public async Task<bool> HasUserAppliedToJobAsync(int applicantId, int jobId)
        {
            var spec = new ApplicationWithFilterSpecification(applicantId, jobId);
            var isExisted = await _unitOfWork.Repository<CandidateApplication>().ExistsAsync(spec);

            return isExisted;
        }

        // ---------------------Recruiter METHODS--------------------
        /// Method for getting applications for Recruiter's jobs
        public async Task<IEnumerable<RecruiterApplicationListDto>> GetApplicationsForRecruiterJobsAsync(int RecruiterId, ApplicationFilterParams filterParams)
        {
            var spec = new ApplicationForRecruiterJobsSpecification(RecruiterId, filterParams);
            var applications = await _unitOfWork.Repository<CandidateApplication>().GetAllAsync(spec);
            var mappedApplications = _mapper.Map<IEnumerable<RecruiterApplicationListDto>>(applications);
            return mappedApplications;
        }

        /// Get all applications for a specific job 
        public async Task<IEnumerable<RecruiterApplicationListDto>> GetApplicationsByJobIdAsync(int jobId, int RecruiterId)
        {
            // First verify that the job belongs to this Recruiter
            var jobSpec = new JobFinderSpecification(jobId);
            var job = await _unitOfWork.Repository<Job>().GetByIdAsync(jobSpec);

            if (job == null || job.RecruiterId != RecruiterId)
                return Enumerable.Empty<RecruiterApplicationListDto>();

            var filterParams = new ApplicationFilterParams
            {
                JobId = jobId
            };

            var spec = new ApplicationForRecruiterJobsSpecification(RecruiterId, filterParams);
            var applications = await _unitOfWork.Repository<CandidateApplication>().GetAllAsync(spec);
            var mappedApplications = _mapper.Map<IEnumerable<RecruiterApplicationListDto>>(applications);
            return mappedApplications;
        }

        /// Method for updating application status
        public async Task<bool> UpdateApplicationStatusAsync(int applicationId, ApplicationStatus status, int RecruiterId)
        {
            // Retrieve the application with job information
            var spec = new ApplicationWithFilterSpecification(applicationId);
            var applications = await _unitOfWork.Repository<CandidateApplication>().GetAllAsync(spec);
            var application = applications.FirstOrDefault();

            // Validate application existence and Recruiter ownership
            if (application == null || application.Job?.RecruiterId != RecruiterId)
                return false;

            // Check if the status is actually changing
            var oldStatus = application.Status;
            if (oldStatus == status)
                return false;

            // Update status
            application.Status = status;
            _unitOfWork.Repository<CandidateApplication>().Update(application);
            var result = await _unitOfWork.CompleteAsync();

            if (result > 0)
            {
                // Generate notification message
                var notificationMessage = GetNotificationMessage(status, application.Job?.Title);
                var applicationLink = $"/applicationDtl/{application.Id}";

                // Send notification to the applicant
                await _notificationService.AddNotificationAsync(
                    application.Applicant.UserId,
                    notificationMessage,
                    applicationLink
                );
            }
            return result > 0;
        }

        // ---------------------ADMIN METHODS--------------------
        /// Delete an application (admin only)
        public async Task<bool> DeleteApplicationAsync(int id)
        {
            var application = await _unitOfWork.Repository<CandidateApplication>().GetByIdAsync(id);
            if (application == null)
                return false;

            _unitOfWork.Repository<CandidateApplication>().Delete(application);

            var deleted = await _unitOfWork.CompleteAsync();
            return deleted > 0;
        }

        // ---------------------MUTUAL METHODS--------------------
        /// Gets a specific application by ID (accessible by Candidate, Recruiter or admin)
        public async Task<ApplicationDto> GetApplicationByIdAsync(int id)
        {
            var spec = new ApplicationWithFilterSpecification(id);
            var application = await _unitOfWork.Repository<CandidateApplication>().GetByIdAsync(spec);
            var mappedApplication = _mapper.Map<ApplicationDto>(application);
            return mappedApplication;
        }

        // ---------------------PRIVATE HELPER METHODS--------------------
        /// Sends notification to Recruiter when a new application is received
        private async Task SendApplicationNotificationAsync(Job job, string applicantName, int applicationId)
        {
            var notificationMessage = $"{applicantName} has applied for the job: {job.Title}";
            var applicationLink = $"/appView/{applicationId}";
            await _notificationService.AddNotificationAsync(job.Recruiter.UserId, notificationMessage, applicationLink);
        }

        /// Method for generating appropriate notification messages based on application status
        private string GetNotificationMessage(ApplicationStatus status, string jobTitle)
        {
            switch (status)
            {
                case ApplicationStatus.Accepted:
                    return $"🎉 Congratulations! Your application for '{jobTitle}' has been accepted.";

                case ApplicationStatus.Rejected:
                    return $"🙏 Thank you for your interest. Your application for '{jobTitle}' was not selected at this time.";

                case ApplicationStatus.UnderReview:
                    return $"🔎 Your application for '{jobTitle}' is now under review.";

                case ApplicationStatus.Interviewed:
                    return $"📅 You have been selected for an interview for '{jobTitle}'.";

                default:
                    return $"Your application for '{jobTitle}' has been updated.";
            }
        }
    }

}
