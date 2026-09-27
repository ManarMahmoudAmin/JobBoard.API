using AutoMapper;
using JobBoard.Application.DTOs;
using JobBoard.Application.Interfaces;
using JobBoard.Application.Specifications.RecruiterSpecifications;
using JobBoard.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Features.Recruiter.Query.GetByUserIdForRecruiter
{
    internal class GetByUserIdForRecruiterHandler : IRequestHandler<GetByUserIdForRecruiterQuery, RecruiterProfileDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetByUserIdForRecruiterHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RecruiterProfileDto?> Handle(GetByUserIdForRecruiterQuery request, CancellationToken cancellationToken)
        {
            var spec = new RecruiterSpecifications(request.UserId);

            var recruiter = await _unitOfWork.Repository<RecruiterProfile>()
                .GetByIdAsync(spec);

            if (recruiter == null)
                return null;

            return _mapper.Map<RecruiterProfileDto>(recruiter);
        }
    }
}
