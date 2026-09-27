using JobBoard.Application.Interfaces;
using JobBoard.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Features.CandidatApplications.Commands.DeleteApplication
{
    internal class DeleteApplicationHandler : IRequestHandler<DeleteApplicationCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteApplicationHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteApplicationCommand request, CancellationToken cancellationToken)
        {
            var application = await _unitOfWork.Repository<CandidateApplication>().
                GetByIdAsync(request.id);
            if (application == null)
                return false;

            _unitOfWork.Repository<CandidateApplication>().Delete(application);

            var deleted = await _unitOfWork.CompleteAsync();
            return deleted > 0;
        }
    }
}
