using AutoMapper;
using JobBoard.Application.DTOs.SavedJobDTOs;
using JobBoard.Application.Interfaces;
using JobBoard.Application.Shared;
using JobBoard.Application.Specifications;
using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Services
{
    public class SavedJobService : ISavedJobService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SavedJobService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<SavedJobDto>> GetSavedJobsAsync(int CandidateId, SavedJobFilterParams filterParams)
        {
            var spec = new SavedJobWithFilterSpecification(CandidateId, filterParams);
            var savedJobs = await _unitOfWork.Repository<SavedJob>().GetAllAsync(spec);
            var mappedSavedJobs = _mapper.Map<IEnumerable<SavedJobDto>>(savedJobs);
            return mappedSavedJobs;
        }

        public async Task<SavedJobDto> GetSavedJobByIdAsync(int CandidateId, int jobId)
        {
            var savedJob = await _unitOfWork.Repository<SavedJob>()
                .FindAsync(s => s.CandidateId == CandidateId && s.JobId == jobId);
            if (savedJob == null)
                return null;

            var spec = new SavedJobWithFilterSpecification(savedJob.Id);
            var savedJobWithDetails = await _unitOfWork.Repository<SavedJob>().GetByIdAsync(spec);
            var mappedSavedJob = _mapper.Map<SavedJobDto>(savedJobWithDetails);
            return mappedSavedJob;
        }

        public async Task<SavedJobDto> SaveJobAsync(int CandidateId, CreateSavedJobDto savedJobDto)
        {
            var alreadySaved = await _unitOfWork.Repository<SavedJob>()
                .AnyAsync(s => s.CandidateId == CandidateId && s.JobId == savedJobDto.JobId);
            if (alreadySaved)
                return null;

            var jobExists = await _unitOfWork.Repository<Job>()
                .AnyAsync(j => j.Id == savedJobDto.JobId);

            if (!jobExists)
                return null;

            var savedJob = _mapper.Map<SavedJob>(savedJobDto);
            savedJob.CandidateId = CandidateId;

            await _unitOfWork.Repository<SavedJob>().AddAsync(savedJob);
            await _unitOfWork.CompleteAsync();

            var spec = new SavedJobWithFilterSpecification(savedJob.Id);
            savedJob = await _unitOfWork.Repository<SavedJob>().GetByIdAsync(spec);
            var mappedSavedJob = _mapper.Map<SavedJobDto>(savedJob);
            return mappedSavedJob;
        }

        public async Task<bool> IsJobSavedAsync(int CandidateId, int jobId)
        {
            var isSaved = await _unitOfWork.Repository<SavedJob>()
                .AnyAsync(s => s.CandidateId == CandidateId && s.JobId == jobId);
            return isSaved;
        }

        public async Task<bool> UnsaveJobAsync(int CandidateId, int jobId)
        {
            var savedJob = await _unitOfWork.Repository<SavedJob>()
           .FindAsync(s => s.CandidateId == CandidateId && s.JobId == jobId);

            if (savedJob == null)
                return false;

            _unitOfWork.Repository<SavedJob>().Delete(savedJob);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
