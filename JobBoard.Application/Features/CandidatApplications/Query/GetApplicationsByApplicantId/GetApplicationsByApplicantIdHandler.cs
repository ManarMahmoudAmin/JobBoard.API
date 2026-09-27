using AutoMapper;
using JobBoard.Application.DTOs.CandidateApplicationDTOs;
using JobBoard.Application.Interfaces;
using JobBoard.Application.Specifications.ApplicationSpecifications;
using JobBoard.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Features.CandidatApplications.Query.GetApplicationsByApplicantId
{
    internal class GetApplicationsByApplicantIdHandler :
        IRequestHandler<GetApplicationsByApplicantIdQuery, IEnumerable<CandidateApplicationListDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetApplicationsByApplicantIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CandidateApplicationListDto>> Handle(GetApplicationsByApplicantIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new ApplicationWithFilterSpecification(request.ApplicantId, isApplicantId: true);
            var applications = await _unitOfWork.Repository<CandidateApplication>().GetAllAsync(spec);
            var mappedApplications = _mapper.Map<IEnumerable<CandidateApplicationListDto>>(applications);
            return mappedApplications;
        }
    }
}
