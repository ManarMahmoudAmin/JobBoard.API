using AutoMapper;
using JobBoard.Application.DTOs;
using JobBoard.Application.DTOs.RecruiterDTOs;
using JobBoard.Application.Interfaces;
using JobBoard.Application.Specifications.RecruiterSpecifications;
using JobBoard.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace JobBoard.Application.Services
{
    public class RecruiterService : IRecruiterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IDocumentService _documentService;
        private readonly IConfiguration _configuration;

        public RecruiterService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IDocumentService documentService,
            IFileService fileService,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
            _documentService = documentService;
            _configuration = configuration;
        }

        ///////////////////////// Delete Recruiter profile ///////////////////////

        public async Task<bool> DeleteById(int id)
        {
            var recruiter = await _unitOfWork
                .Repository<RecruiterProfile>()
                .FindAsync(r => r.Id == id);

            if (recruiter == null)
                return false;

            var defaultCompanyImage = $"{_configuration["ApiBaseUrl"]}/images/companies/default.jpg";

            // Delete company image only if it's not the default
            if (!string.IsNullOrEmpty(recruiter.CompanyImage) &&
                !_fileService.IsDefaultImage(
                    recruiter.CompanyImage,
                    defaultCompanyImage))
            {
                _documentService.DeleteFile(recruiter.CompanyImage,
                    "images/companies");
            }

            _unitOfWork
                .Repository<RecruiterProfile>()
                .Delete(recruiter);

            await _unitOfWork.CompleteAsync();

            return true;
        }

        ///////////////////////// Get all Recruiters ///////////////////////

        public async Task<IEnumerable<RecruiterProfileDto>> GetAll()
        {
            var spec = new RecruiterSpecifications();

            var recruiters = await _unitOfWork.Repository<RecruiterProfile>()
                .GetAllAsync(spec);

            return _mapper.Map<List<RecruiterProfileDto>>(recruiters);
        }

        ///////////////////////// Update Recruiter profile ///////////////////////

        public async Task<bool> Update(
            int id,
            RecruiterProfileUpdateDto model)
        {
            var spec = new RecruiterForUpdateSpecification(id);

            var recruiter = await _unitOfWork.Repository<RecruiterProfile>()
                 .GetByIdAsync(spec);

            if (recruiter == null)
                return false;

            var defaultCompanyImage =
                $"{_configuration["ApiBaseUrl"]}/images/companies/default.jpg";

            // Handle Company Image upload/removal
            recruiter.CompanyImage =
                await _fileService.HandleFileUploadAsync(
                    model.CompanyImage,
                    recruiter.CompanyImage,
                    "images/companies",
                    model.RemoveCompanyImage,
                    defaultCompanyImage);

            // Update Recruiter profile using AutoMapper
            _mapper.Map(model, recruiter);

            // Update related User entity manually if needed
            if (!string.IsNullOrEmpty(model.Phone))
            {
                recruiter.User.PhoneNumber = model.Phone;
            }

            await _unitOfWork.CompleteAsync();

            return true;
        }

        ///////////////////////// Get Recruiter by user id ///////////////////////

        public async Task<RecruiterProfileDto?> GetByUserId(string userId)
        {
            var spec = new RecruiterSpecifications(userId);

            var recruiter = await _unitOfWork.Repository<RecruiterProfile>()
                .GetByIdAsync(spec);

            if (recruiter == null)
                return null;

            return _mapper.Map<RecruiterProfileDto>(recruiter);
        }
    }
}
