using AutoMapper;
using JobBoard.Application.DTOs;
using JobBoard.Application.DTOs.AdminDTOs;
using JobBoard.Application.DTOs.CandidateDTOs;
using JobBoard.Application.DTOs.JobDTOs;
using JobBoard.Application.Interfaces;
using JobBoard.Application.Specifications;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IAIEmbeddingService _aiEmbeddingService;
        private readonly INotificationService _notificationService;
        private readonly IRedisService _redisService;

        public AdminService(IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            IAIEmbeddingService aiEmbeddingService,
            INotificationService notificationService,
            IRedisService redisService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            _aiEmbeddingService = aiEmbeddingService;
            _notificationService = notificationService;
            _redisService = redisService;
        }

        ///////////////////////get all Candidates///////////////////////
        public async Task<List<CandidateProfileDto>> GetAllCandidatesAsync()
        {
            var spec = new AllCandidatesSpecification();
            var Candidates = await _unitOfWork.Repository<CandidateProfile>().GetAllAsync(spec);
            return _mapper.Map<List<CandidateProfileDto>>(Candidates);
        }

        ///////////////////////get all Recruiters///////////////////////
        public async Task<List<RecruiterProfileDto>> GetAllRecruitersAsync()
        {
            var spec = new AllRecruitersSpecification();
            var Recruiters = await _unitOfWork.Repository<RecruiterProfile>().GetAllAsync(spec);
            return _mapper.Map<List<RecruiterProfileDto>>(Recruiters);
        }

        ///////////////////////get Candidate by id///////////////////////
        public async Task<CandidateProfileDto> GetCandidateByIdAsync(string userId)
        {
            var spec = new CandidateByUserIdSpecification(userId);
            var Candidate = await _unitOfWork.Repository<CandidateProfile>().GetByIdAsync(spec);
            return _mapper.Map<CandidateProfileDto>(Candidate);
        }


        ///////////////////////get Recruiter by id///////////////////////

        public async Task<RecruiterProfileDto> GetRecruiterByIdAsync(string userId)
        {
            var spec = new RecruiterByUserIdSpecification(userId);
            var Recruiter = await _unitOfWork.Repository<RecruiterProfile>().GetByIdAsync(spec);
            return _mapper.Map<RecruiterProfileDto>(Recruiter);
        }


        ///////////////////////get all jobs///////////////////////
        public async Task<List<JobDto>> GetAllJobsAsync()
        {
            var spec = new AllJobsSpecification();
            var jobs = await _unitOfWork.Repository<Job>().GetAllAsync(spec);
            return _mapper.Map<List<JobDto>>(jobs);
        }


        ////////////////////////delete job by id///////////////////////
        public async Task<bool> DeleteJob(int id)
        {
            var job = await _unitOfWork.Repository<Job>().GetByIdAsync(new JobByIdWithApplication(id));
            if (job == null)
                return false;

            foreach (var app in job.JobApplications)
            {
                _unitOfWork.Repository<CandidateApplication>().Delete(app);
            }

            _unitOfWork.Repository<Job>().Delete(job);
            await _unitOfWork.CompleteAsync();
            await _aiEmbeddingService.DeleteEmbeddingForJobAsync(id);
            //await _redisService.DeleteByPrefixAsync("jobs:");
            //await _outputCacheStore.EvictByTagAsync("jobs", default);

            return true;
        }


        ///////////////////////get all pending jobs///////////////////////
        public async Task<List<JobDto>> GetPendingJobsAsync()
        {
            var spec = new PendingJobsSpecification();
            var jobs = await _unitOfWork.Repository<Job>().GetAllAsync(spec);
            return _mapper.Map<List<JobDto>>(jobs);

        }


        ///////////////////////approve job by id///////////////////////
        public async Task<bool> ApproveJobAsync(int jobId)
        {
            var spec = new JobByIdSpecification(jobId);
            var job = await _unitOfWork.Repository<Job>().GetByIdAsync(spec);

            if (job == null)
                return false;
            //Check if job is already approved
            if (job.IsApproved)
                return false;

            job.IsApproved = true;
            job.PostedDate = DateTime.Now;
            _unitOfWork.Repository<Job>().Update(job);

            var result = await _unitOfWork.CompleteAsync();

            var notificationMessage = $"Your job {job.Title} has been approved!";
            var jobLink = $"/jobDtl/{job.Id}";

            //remove job from redis cache
            await _aiEmbeddingService.GenerateEmbeddingForJobAsync(job);
            await _redisService.DeleteByPrefixAsync("jobs:");
            //await _outputCacheStore.EvictByTagAsync("jobs", default);


            await _notificationService.AddNotificationAsync(job.Recruiter.UserId, notificationMessage, jobLink);
            return result > 0;
        }



        //////////////////////reject job by id///////////////////////
        public async Task<bool> RejectJobAsync(int jobId)
        {
            var job = await _unitOfWork.Repository<Job>().GetByIdAsync(new JobByIdWithApplication(jobId));

            if (job == null) return false;
            foreach (var app in job.JobApplications)
            {
                _unitOfWork.Repository<CandidateApplication>().Delete(app);
            }

            _unitOfWork.Repository<Job>().Delete(job);

            var result = await _unitOfWork.CompleteAsync();
            if (job.Recruiter != null)
            {
                var notificationMessage = $"Your job {job.Title} has been rejected!";
                await _notificationService.AddNotificationAsync(job.Recruiter.UserId, notificationMessage);
            }

            await _aiEmbeddingService.DeleteEmbeddingForJobAsync(jobId);
            return result > 0;
        }

        //////////////////////delete user by id///////////////////////
        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            if (user.User_Type == UserType.Candidate)
            {
                var result = await _userManager.DeleteAsync(user);
                return result.Succeeded;
            }

            else if (user.User_Type == UserType.Recruiter)
            {
                var RecruiterSpec = new RecruiterByUserIdSpecification(userId);
                var Recruiter = await _unitOfWork.Repository<RecruiterProfile>().GetByIdAsync(RecruiterSpec);

                if (Recruiter != null)
                {
                    Recruiter.IsDeleted = true;

                    var jobs = await _unitOfWork.Repository<Job>()
                        .GetAllAsync(new JobsByRecruiterIdSpecification(Recruiter.Id));

                    foreach (var job in jobs)
                    {
                        job.IsDeleted = true;
                        await _aiEmbeddingService.DeleteEmbeddingForJobAsync(job.Id);

                        foreach (var app in job.JobApplications)
                        {
                            _unitOfWork.Repository<CandidateApplication>().Delete(app);
                        }
                    }

                    _unitOfWork.Repository<RecruiterProfile>().Update(Recruiter);
                    await _unitOfWork.CompleteAsync();
                }

                var deleteResult = await _userManager.DeleteAsync(user);
                await _redisService.DeleteByPrefixAsync("admin:");

                return deleteResult.Succeeded;
            }

            return false;
        }

        //////////////////////get stats///////////////////////
        public async Task<StatsDto> GetStatsAsync()
        {
            var CandidatesCount = (await _userManager.GetUsersInRoleAsync(UserType.Candidate.ToString())).Count;
            var RecruitersCount = (await _userManager.GetUsersInRoleAsync(UserType.Recruiter.ToString())).Count;

            // Using repository for counting jobs
            var jobsCount = await _unitOfWork.Repository<Job>().CountAsync();

            var pendingJobsSpec = new PendingJobsSpecification();
            var pendingJobsCount = await _unitOfWork.Repository<Job>().CountAsync(pendingJobsSpec);

            return new StatsDto()
            {
                TotalCandidates = CandidatesCount,
                TotalRecruiters = RecruitersCount,
                TotalJobs = jobsCount,
                PendingJobs = pendingJobsCount
            };
        }
    }

}
