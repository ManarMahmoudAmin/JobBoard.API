using AutoMapper;
using JobBoard.Application.DTOs.CandidateApplicationDTOs;
using JobBoard.Application.Interfaces;
using JobBoard.Application.Shared;
using JobBoard.Application.Specifications.ApplicationSpecifications;
using JobBoard.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Features.CandidatApplications.Query.GetApplicationsByJobId
{
    internal class GetApplicationsByJobIdHandler :
        IRequestHandler<GetApplicationsByJobIdQuery, IEnumerable<RecruiterApplicationListDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetApplicationsByJobIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<RecruiterApplicationListDto>> Handle(GetApplicationsByJobIdQuery request, CancellationToken cancellationToken)
        {
            var jobSpec = new JobFinderSpecification(request.JobId);
            var job = await _unitOfWork.Repository<Job>().GetByIdAsync(jobSpec);

            if (job == null || job.RecruiterId != request.RecruiterId)
                return Enumerable.Empty<RecruiterApplicationListDto>();

            var filterParams = new ApplicationFilterParams
            {
                JobId = request.JobId
            };

            var spec = new ApplicationForRecruiterJobsSpecification(request.RecruiterId, filterParams);
            var applications = await _unitOfWork.Repository<CandidateApplication>().GetAllAsync(spec);
            var mappedApplications = _mapper.Map<IEnumerable<RecruiterApplicationListDto>>(applications);
            return mappedApplications;
        }
    }
}
