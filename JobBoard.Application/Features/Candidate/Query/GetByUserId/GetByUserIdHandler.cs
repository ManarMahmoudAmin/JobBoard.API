using AutoMapper;
using JobBoard.Application.DTOs.CandidateDTOs;
using JobBoard.Application.Interfaces;
using JobBoard.Application.Specifications;
using JobBoard.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Features.Candidate.Query.GetByUserId
{
    internal class GetByUserIdHandler : IRequestHandler<GetByUserIdQuery, CandidateProfileDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetByUserIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<CandidateProfileDto?> Handle(GetByUserIdQuery request, CancellationToken cancellationToken)
        {
            var specification = new CandidateByUserIdSpecification(request.userId);

            var candidate = await _unitOfWork.Repository<CandidateProfile>()
                .GetByIdAsync(specification);

            if (candidate == null)
                return null;

            return _mapper.Map<CandidateProfileDto>(candidate);
        }
    }
}
