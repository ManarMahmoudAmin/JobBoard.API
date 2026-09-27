using AutoMapper;
using JobBoard.Application.DTOs.CandidateApplicationDTOs;
using JobBoard.Application.Interfaces;
using JobBoard.Application.Specifications.ApplicationSpecifications;
using JobBoard.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Features.CandidatApplications.Query.GetApplicationsForRecruiterJobs
{
    internal class GetApplicationsForRecruiterJobsHandler :
        IRequestHandler<GetApplicationsForRecruiterJobsQuery, IEnumerable<RecruiterApplicationListDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetApplicationsForRecruiterJobsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<RecruiterApplicationListDto>> Handle(GetApplicationsForRecruiterJobsQuery request, CancellationToken cancellationToken)
        {
            var spec = new ApplicationForRecruiterJobsSpecification(request.RecruiterId, request.filterParams);
            var applications = await _unitOfWork.Repository<CandidateApplication>().GetAllAsync(spec);
            var mappedApplications = _mapper.Map<IEnumerable<RecruiterApplicationListDto>>(applications);
            return mappedApplications;
        }
    }
}
