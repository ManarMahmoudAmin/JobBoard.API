using AutoMapper;
using JobBoard.Application.DTOs.JobDTOs;
using JobBoard.Application.DTOs.SkillAndCategoryDTOs;
using JobBoard.Application.Interfaces;
using JobBoard.Application.Shared;
using JobBoard.Application.Specifications.DashboardSpecifications;
using JobBoard.Application.Specifications.JobSpecifications;
using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Services
{
    public class JobService : IJobService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAIEmbeddingService _aiEmbeddingService;
        private readonly IRedisService _redisService;

        public JobService(IUnitOfWork unitOfWork, IMapper mapper, IAIEmbeddingService aiEmbeddingService, IRedisService redisService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _aiEmbeddingService = aiEmbeddingService;
            _redisService = redisService;
        }
        public async Task<IEnumerable<JobListDto>> GetAllJobsAsync(JobFilterParams filterParams)
        {
            var spec = new JobsWithFilterSpecifications(filterParams);
            var jobs = await _unitOfWork.Repository<Job>().GetAllAsync(spec);
            var mappedJobs = _mapper.Map<IEnumerable<JobListDto>>(jobs);
            return mappedJobs;

        }
        public async Task<IEnumerable<PostedJobsDto>> GetRecruiterJobsAsync(int RecruiterId, RecruiterJobFilterParams filterParams)
        {
            var spec = new RecruiterJobsWithFilterSpecification(RecruiterId, filterParams);
            var jobs = await _unitOfWork.Repository<Job>().GetAllAsync(spec);
            var mappedJobs = _mapper.Map<IEnumerable<PostedJobsDto>>(jobs);

            return mappedJobs;
        }

        public async Task<IEnumerable<TopPerformingJobDto>> GetTopPerformingJobsAsync(int RecruiterId, int limit = 5)
        {
            var spec = new TopPerformingJobsSpecification(RecruiterId, limit);
            var jobs = await _unitOfWork.Repository<Job>().GetAllAsync(spec);
            var mappedJobs = _mapper.Map<IEnumerable<TopPerformingJobDto>>(jobs);
            return mappedJobs;
        }

        public async Task<IEnumerable<RecentJobDto>> GetRecentJobsAsync(int RecruiterId, int limit = 3)
        {
            var spec = new RecentJobsSpecification(RecruiterId, limit);
            var jobs = await _unitOfWork.Repository<Job>().GetAllAsync(spec);
            var mappedJobs = _mapper.Map<IEnumerable<RecentJobDto>>(jobs);
            return mappedJobs;
        }

        public async Task<JobDto> GetJobByIdAsync(int id)
        {
            var spec = new JobsWithFilterSpecifications(id);
            var job = await _unitOfWork.Repository<Job>().GetByIdAsync(spec);
            var mappedJob = _mapper.Map<JobDto>(job);
            return mappedJob;
        }


        public async Task<JobDto> AddJobAsync(CreateUpdateJobDto jobDto, int RecruiterId)
        {
            var job = _mapper.Map<Job>(jobDto);
            job.RecruiterId = RecruiterId;
            job.IsApproved = false;

            await MapSkillsAndCategoriesAsync(job, jobDto);

            await _unitOfWork.Repository<Job>().AddAsync(job);
            await _unitOfWork.CompleteAsync();

            await _aiEmbeddingService.GenerateEmbeddingForJobAsync(job);


            //await _redisService.DeleteByPrefixAsync("jobs:");

            return _mapper.Map<JobDto>(job);
        }

        public async Task<JobDto> UpdateJob(int id, CreateUpdateJobDto jobDto, int RecruiterId)
        {
            var spec = new JobUpdateSpecification(id, RecruiterId);
            var job = await _unitOfWork.Repository<Job>().GetByIdAsync(spec);

            if (job == null)
                return null;

            _mapper.Map(jobDto, job);

            if (job.IsApproved)
            {
                job.IsApproved = false;
            }

            await MapSkillsAndCategoriesAsync(job, jobDto);
            _unitOfWork.Repository<Job>().Update(job);
            await _unitOfWork.CompleteAsync();
            await _aiEmbeddingService.GenerateEmbeddingForJobAsync(job);
            //await _redisService.DeleteByPrefixAsync("jobs:");

            return _mapper.Map<JobDto>(job);
        }

        public async Task<bool> DeleteJob(int id, int RecruiterId)
        {
            var job = await _unitOfWork.Repository<Job>().GetByIdAsync(id);
            if (job == null)
                return false;

            if (job.RecruiterId != RecruiterId)
                return false;

            _unitOfWork.Repository<Job>().Delete(job);
            await _unitOfWork.CompleteAsync();
            await _aiEmbeddingService.DeleteEmbeddingForJobAsync(id);
            //await _redisService.DeleteByPrefixAsync("jobs:");

            return true;
        }


        private async Task MapSkillsAndCategoriesAsync(Job job, CreateUpdateJobDto jobDto)
        {

            // Categories
            if (job.Categories == null)
                job.Categories = new List<Category>();

            job.Categories.Clear();

            if (jobDto.CategoryIds?.Count > 0)
            {
                var spec = new CategoriesByIdsSpecifications(jobDto.CategoryIds);
                var categories = await _unitOfWork.Repository<Category>().GetAllAsync(spec);
                job.Categories = categories.ToList();
            }
            // Skills
            if (job.Skills == null)
                job.Skills = new List<Skill>();
            job.Skills.Clear();

            if (jobDto.SkillIds?.Count > 0)
            {
                var spec = new SkillsByIdsSpecifications(jobDto.SkillIds);
                var skills = await _unitOfWork.Repository<Skill>().GetAllAsync(spec);
                job.Skills = skills.ToList();
            }
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Repository<Category>().GetAllAsync();
            var mappedCategories = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            return mappedCategories;
        }

        public async Task<IEnumerable<SkillDto>> GetAllSkillsAsync()
        {
            var skills = await _unitOfWork.Repository<Skill>().GetAllAsync();
            var mappedSkills = _mapper.Map<IEnumerable<SkillDto>>(skills);

            return mappedSkills;
        }
    }

}
