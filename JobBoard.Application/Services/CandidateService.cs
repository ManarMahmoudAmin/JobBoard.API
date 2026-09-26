using AutoMapper;
using JobBoard.Application.DTOs.CandidateDTOs;
using JobBoard.Application.Interfaces;
using JobBoard.Application.Shared;
using JobBoard.Application.Specifications;
using JobBoard.Application.Specifications.CandidateSpecifications;
using JobBoard.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace JobBoard.Application.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        private readonly IDocumentService _documentService;
        private readonly IFileService _fileService;

        public CandidateService(IUnitOfWork unitOfWork, 
            IMapper mapper,
            IConfiguration configuration,
            IDocumentService documentService, 
            IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _documentService = documentService;
            _fileService = fileService;
        }

        // =================== GET BY USER ID ===================

        public async Task<CandidateProfileDto?> GetByUserIdAsync(string userId)
        {
            var specification = new CandidateByUserIdSpecification(userId);

            var candidate = await _unitOfWork.Repository<CandidateProfile>()
                .GetByIdAsync(specification);

            if (candidate == null)
                return null;

            return _mapper.Map<CandidateProfileDto>(candidate);
        }


        // =================== GET ALL ===================

        public async Task<IEnumerable<CandidateProfileDto>> GetAllAsync()
        {
            var candidates = await _unitOfWork.Repository<CandidateProfile>()
                .GetAllAsync();

            return _mapper.Map<List<CandidateProfileDto>>(candidates);
        }


        // =================== UPDATE ===================

        public async Task<bool> UpdateAsync(string userId, CandidateProfileUpdateDto dto)
        {
            var spec = new CandidateForUpdateSpecification(userId);

            var candidate = await _unitOfWork.Repository<CandidateProfile>().GetByIdAsync(spec);

            if (candidate == null)
                return false;

            MapBasicInfo(dto, candidate);

            await UpdateSkillsAsync(dto.Skills, candidate);

            UpdateInterests(dto.Interests, candidate);
            UpdateCertificates(dto.Certificates, candidate);
            UpdateTrainings(dto.Trainings, candidate);
            UpdateExperiences(dto.CandidateExperiences, candidate);
            UpdateEducations(dto.CandidateEducations, candidate);

            await _unitOfWork.CompleteAsync();

            return true;
        }


        private void MapBasicInfo(CandidateProfileUpdateDto dto, CandidateProfile candidate)
        {
            _mapper.Map(dto, candidate);

            if (!string.IsNullOrEmpty(dto.PhoneNumber))
                candidate.User.PhoneNumber = dto.PhoneNumber;

            if (!string.IsNullOrEmpty(dto.Name))
                candidate.User.UserName = dto.Name;
        }


        private async Task UpdateSkillsAsync(IEnumerable<string?> skills,CandidateProfile candidate)
        {
            if (skills == null)
                return;

            // Remove duplicates ignoring case
            var updatedSkills = skills.Where(s => !string.IsNullOrWhiteSpace(s)).
                Select(s => s!).Distinct(StringComparer.OrdinalIgnoreCase).ToList();

            candidate.Skills.Clear();

            // Get existing skills from database
            var spec = new CandidateSkillsSpecification(updatedSkills);

            var existingSkillsInDb = await _unitOfWork.Repository<Skill>().GetAllAsync(spec);
            
            var existingSkills = existingSkillsInDb.ToDictionary(s => s.SkillName.ToLower(), s => s);

            foreach (var skillName in updatedSkills)
            {
                var key = skillName.ToLower();

                if (existingSkills.TryGetValue(key, out var existingSkill))
                {
                    candidate.Skills.Add(existingSkill);
                }
                else
                {
                    var newSkill = new Skill
                    {
                        SkillName = skillName
                    };

                    candidate.Skills.Add(newSkill);
                    existingSkills[key] = newSkill;
                }
            }
        }

        private void UpdateInterests(List<string>? interests, CandidateProfile candidate)
        {
            if (interests == null)
                return;

            var current = candidate.CandidateInterests.Select(i => i.InterestName).ToList();

            var updated = interests.Distinct().ToList();

            var toRemove = candidate.CandidateInterests.Where(i => !updated.Contains(i.InterestName))
                .ToList();

            foreach (var interest in toRemove)
                candidate.CandidateInterests.Remove(interest);

            foreach (var interestName in updated)
            {
                if (!current.Contains(interestName))
                {
                    candidate.CandidateInterests.Add(
                        new CandidateInterest
                        {
                            InterestName = interestName
                        });
                }
            }
        }

        private void UpdateCertificates(List<string>? certificates, CandidateProfile candidate)
        {
            if (certificates == null)
                return;

            var current = candidate.CandidateCertificates.Select(c => c.CertificateName).ToList();

            var updated = certificates.Distinct().ToList();

            var toRemove = candidate.CandidateCertificates
                .Where(c => !updated.Contains(c.CertificateName)).ToList();

            foreach (var cert in toRemove)
                candidate.CandidateCertificates.Remove(cert);

            foreach (var certName in updated)
            {
                if (!current.Contains(certName))
                {
                    candidate.CandidateCertificates.Add(
                        new CandidateCertificate
                        {
                            CertificateName = certName
                        });
                }
            }
        }

        private void UpdateTrainings(List<string>? trainings, CandidateProfile candidate)
        {
            if (trainings == null)
                return;

            var current = candidate.CandidateTraining.Select(t => t.TrainingName).ToList();

            var updated = trainings.Distinct().ToList();

            var toRemove = candidate.CandidateTraining
                .Where(t => !updated.Contains(t.TrainingName)).ToList();

            foreach (var training in toRemove)
                candidate.CandidateTraining.Remove(training);

            foreach (var trainingName in updated)
            {
                if (!current.Contains(trainingName))
                {
                    candidate.CandidateTraining.Add(
                        new CandidateTraining
                        {
                            TrainingName = trainingName
                        });
                }
            }
        }


        private void UpdateExperiences(List<CandidateExperienceUpdateDto>? experiences, 
            CandidateProfile candidate)
        {
            if (experiences == null)
                return;

            var currentExperiences = candidate.CandidateExperiences.ToList();

            var dtoIds = experiences
                .Where(e => e.Id != null).Select(e => e.Id).ToList();

            var toRemove = currentExperiences.Where(e => !dtoIds.Contains(e.Id)).ToList();

            foreach (var exp in toRemove)
                candidate.CandidateExperiences.Remove(exp);

            foreach (var expDto in experiences)
            {
                if (expDto.Id == null)
                {
                    candidate.CandidateExperiences.Add(
                        _mapper.Map<CandidateExperience>(expDto));
                }
                else
                {
                    var existingExp = currentExperiences.FirstOrDefault(e => e.Id == expDto.Id);

                    if (existingExp != null)
                        _mapper.Map(expDto, existingExp);
                }
            }
        }
        private void UpdateEducations(List<CandidateEducationUpdateDto>? educations,
            CandidateProfile candidate)
        {
            if (educations == null)
                return;

            var currentEducations =candidate.CandidateEducations.ToList();

            var dtoIds = educations.Where(e => e.Id != null).Select(e => e.Id).ToList();

            var toRemove = currentEducations.Where(e => !dtoIds.Contains(e.Id)).ToList();

            foreach (var edu in toRemove)
                candidate.CandidateEducations.Remove(edu);

            foreach (var eduDto in educations)
            {
                if (eduDto.Id == null)
                {
                    candidate.CandidateEducations.Add(_mapper.Map<CandidateEducation>(eduDto));
                }
                else
                {
                    var existingEdu = currentEducations.FirstOrDefault(e => e.Id == eduDto.Id);

                    if (existingEdu != null)
                        _mapper.Map(eduDto, existingEdu);
                }
            }
        }

        // =================== UPLOAD FILES ===================

        public async Task<(string? CvUrl, string? ProfileImageUrl)>
            UploadFilesAsync(string userId, CandidateFileUploadDto dto)
        {
            var candidate = await _unitOfWork.Repository<CandidateProfile>()
                .FindAsync(s => s.UserId == userId);

            if (candidate == null)
                return (null, null);

            // Handle CV upload/update
            var newCvUrl =
                await _fileService.HandleFileUploadAsync(
                    dto.CV_Url,
                    candidate.CV_Url,
                    "cv",
                    dto.RemoveCV,
                    null);

            // Handle Profile Image upload/removal
            var defaultProfileImage =
                $"{_configuration["ApiBaseUrl"]}/images/profilepic/user.jpg";

            var newProfileImageUrl =
                await _fileService.HandleFileUploadAsync(
                    dto.ProfileImageUrl,
                    candidate.ProfileImageUrl,
                    "images/profilepic",
                    dto.RemoveProfileImage,
                    defaultProfileImage);

            candidate.CV_Url = newCvUrl;
            candidate.ProfileImageUrl = newProfileImageUrl;

            await _unitOfWork.CompleteAsync();

            return (newCvUrl, newProfileImageUrl);
        }


        // =================== DELETE ===================

        public async Task<bool> DeleteAsync(int id)
        {
            var candidate = await _unitOfWork.Repository<CandidateProfile>()
                .GetByIdAsync(id);

            if (candidate == null)
                return false;

            var defaultProfileImage =
                $"{_configuration["ApiBaseUrl"]}/images/profilepic/user.jpg";

            // Delete profile image only if it is not the default
            if (!string.IsNullOrEmpty(candidate.ProfileImageUrl) &&
                !_fileService.IsDefaultImage(candidate.ProfileImageUrl, defaultProfileImage))
            {
                _documentService.DeleteFile(candidate.ProfileImageUrl, "images/profilepic");
            }

            _unitOfWork.Repository<CandidateProfile>().Delete(candidate);

            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
