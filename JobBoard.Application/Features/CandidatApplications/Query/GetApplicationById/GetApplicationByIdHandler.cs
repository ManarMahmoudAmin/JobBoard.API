using AutoMapper;
using JobBoard.Application.DTOs.CandidateApplicationDTOs;
using JobBoard.Application.Interfaces;
using JobBoard.Application.Specifications.ApplicationSpecifications;
using JobBoard.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Features.CandidatApplications.Query.GetApplicationById
{
    internal class GetApplicationByIdHandler : IRequestHandler<GetApplicationByIdQuery, ApplicationDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetApplicationByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApplicationDto> Handle(GetApplicationByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new ApplicationWithFilterSpecification(request.Id);
            var application = await _unitOfWork.Repository<CandidateApplication>().GetByIdAsync(spec);
            var mappedApplication = _mapper.Map<ApplicationDto>(application);
            return mappedApplication;
        }
    }
}
