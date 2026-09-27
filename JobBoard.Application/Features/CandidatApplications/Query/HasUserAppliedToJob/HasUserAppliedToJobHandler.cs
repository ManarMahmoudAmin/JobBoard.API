using JobBoard.Application.Interfaces;
using JobBoard.Application.Specifications.ApplicationSpecifications;
using JobBoard.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Features.CandidatApplications.Query.HasUserAppliedToJob
{
    internal class HasUserAppliedToJobHandler : IRequestHandler<HasUserAppliedToJobQuery, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public HasUserAppliedToJobHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        async Task<bool> IRequestHandler<HasUserAppliedToJobQuery, bool>.Handle(HasUserAppliedToJobQuery request, CancellationToken cancellationToken)
        {
            var spec = new ApplicationWithFilterSpecification(request.ApplicantId, request.JobId);
            var isExisted = await _unitOfWork.Repository<CandidateApplication>().ExistsAsync(spec);

            return isExisted;
        }
    }
}
