using GenerativeAI.Types;
using JobBoard.Application.Interfaces;
using JobBoard.Application.Specifications.ApplicationSpecifications;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace JobBoard.Application.Features.CandidatApplications.Commands.UpdateApplicationStatus
{
    internal class UpdateApplicationStatusHandler : IRequestHandler<UpdateApplicationStatusCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;

        public UpdateApplicationStatusHandler(IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        public async Task<bool> Handle(UpdateApplicationStatusCommand request,
            CancellationToken cancellationToken)
        {
            // Retrieve the application with job information
            var spec = new ApplicationWithFilterSpecification(request.ApplicationId);
            var applications = await _unitOfWork.Repository<CandidateApplication>().GetAllAsync(spec);
            var application = applications.FirstOrDefault();

            // Validate application existence and Recruiter ownership
            if (application == null || application.Job?.RecruiterId != request.RecruiterId)
                return false;

            // Check if the status is actually changing
            var oldStatus = application.Status;
            if (oldStatus == request.Status)
                return false;

            // Update status
            application.Status = request.Status;
            _unitOfWork.Repository<CandidateApplication>().Update(application);
            var result = await _unitOfWork.CompleteAsync();

            if (result > 0)
            {
                // Generate notification message
                var notificationMessage = GetNotificationMessage(request.Status, application.Job?.Title);
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
