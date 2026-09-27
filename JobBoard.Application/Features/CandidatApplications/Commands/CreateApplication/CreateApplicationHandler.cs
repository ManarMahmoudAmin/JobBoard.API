using AutoMapper;
using JobBoard.Application.DTOs.CandidateApplicationDTOs;
using JobBoard.Application.Interfaces;
using JobBoard.Application.Services;
using JobBoard.Application.Specifications.ApplicationSpecifications;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Features.CandidatApplications.Commands.CreateApplication
{
    internal class CreateApplicationHandler :
        IRequestHandler<CreateApplicationCommand, ApplicationDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDocumentService _documentService;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public CreateApplicationHandler(IUnitOfWork unitOfWork,
            IDocumentService documentService, 
            INotificationService notificationService, 
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _documentService = documentService;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        public async Task<ApplicationDto> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
        {
            //Verify if user has already applied to this job
            var appSpec = new ApplicationWithFilterSpecification(request.ApplicantId,
                 request.CreateDto.JobId);
            var hasApplied = await _unitOfWork.Repository<CandidateApplication>()
                .ExistsAsync(appSpec);

            if (hasApplied)
                return null;


            // Get the job details to access Recruiter information
            var jobSpec = new JobFinderSpecification(request.CreateDto.JobId);
            var job = await _unitOfWork.Repository<Job>().GetByIdAsync(jobSpec);
            if (job == null)
                return null;

            var application = _mapper.Map<CandidateApplication>(request.CreateDto);
            application.ApplicantId = request.ApplicantId;
            application.AppliedDate = DateTime.UtcNow;
            application.Status = ApplicationStatus.Pending;

            if (request.CreateDto.ResumeUrl != null && request.CreateDto.ResumeUrl.Length > 0)
            {
                application.ResumeUrl = await _documentService.
                    UploadFileAsync(request.CreateDto.ResumeUrl, "cv");
            }

            await _unitOfWork.Repository<CandidateApplication>().AddAsync(application);

            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0)
                return null;

            // Send notification to Recruiter after successful save
            await SendApplicationNotificationAsync(job, request.CreateDto.FullName, application.Id);

            return _mapper.Map<ApplicationDto>(application);
        }

        private async Task SendApplicationNotificationAsync(Job job, string applicantName, int applicationId)
        {
            var notificationMessage = $"{applicantName} has applied for the job: {job.Title}";
            var applicationLink = $"/appView/{applicationId}";
            await _notificationService.AddNotificationAsync(job.Recruiter.UserId, notificationMessage, applicationLink);
        }

    }
}
