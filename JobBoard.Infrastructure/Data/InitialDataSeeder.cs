using AutoMapper;
using JobBoard.Application.DTOs;
using JobBoard.Application.DTOs.CandidateApplicationDTOs;
using JobBoard.Application.DTOs.CandidateDTOs.CandidateSeedDTOs;
using JobBoard.Application.DTOs.JobDTOs;
using JobBoard.Application.DTOs.RecruiterDTOs;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Enums;
using JobBoard.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JobBoard.Infrastructure.Data
{
    public static class InitialDataSeeder
    {
        private static readonly string DataPath = "../JobBoard.Infrastructure/Data/DataSeed";

        // Options for JSON deserialization, case-insensitive and handling enums
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };

        // The main entry point for seeding all data
        public static async Task SeedAsync(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper)
        {
            // Seed roles first 
            await SeedRolesAsync(roleManager);

            // Seed skills & categories
            await SeedDataAsync<Skill>(context, context.Skills, "skills.json");
            await SeedDataAsync<Category>(context, context.Categories, "categories.json");

            // Seed all users & their profiles
            await SeedUsersAndProfilesAsync(context, userManager, roleManager, mapper);

            // Seed jobs 
            await SeedJobsAsync(context, mapper);

            // seed applications
            await SeedApplicationsAsync(context, mapper);

        }

        // ================= Jobs =================
        private static async Task SeedJobsAsync(ApplicationDbContext context, IMapper mapper)
        {
            if (await context.Jobs.AnyAsync()) return;

            var jobDtos = await LoadJsonFileAsync<JobSeedDto>("jobs.json");
            if (jobDtos == null || !jobDtos.Any()) return;

            var allSkills = await context.Skills.ToListAsync(); // get all skills 
            var allCategories = await context.Categories.ToListAsync(); // get categories
            var allRecruiters = await context.RecruiterProfiles.ToListAsync(); // get Recruiters

            var jobs = jobDtos.Select(dto =>
            {
                var job = mapper.Map<Job>(dto);

                // Match Recruiter by Id or fallback to first one
                job.RecruiterId = allRecruiters.FirstOrDefault(e => e.Id == dto.RecruiterId)?.Id ?? allRecruiters.First().Id;

                // Map Skill IDs to Skill entities
                job.Skills = dto.SkillIds?
                    .Select(id => allSkills.FirstOrDefault(s => s.Id == id))
                    .OfType<Skill>()
                    .ToList() ?? new List<Skill>();

                // Map Category IDs to Category entities
                job.Categories = dto.CategoryIds?
                    .Select(id => allCategories.FirstOrDefault(c => c.Id == id))
                    .OfType<Category>()
                    .ToList() ?? new List<Category>();

                return job;
            }).ToList();

            await context.Jobs.AddRangeAsync(jobs);
            await context.SaveChangesAsync();
        }

        // ================= Users & Profiles =================
        private static async Task SeedUsersAndProfilesAsync(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper)
        {
            var users = await LoadJsonFileAsync<UserSeedDto>("users.json");
            var Recruiters = await LoadJsonFileAsync<RecruiterSeedDto>("recruiters.json");
            var Candidates = await LoadJsonFileAsync<CandidateSeedDto>("candidates.json");

            if (users == null || Recruiters == null || Candidates == null) return;

            foreach (var userDto in users)
            {
                if (await userManager.FindByEmailAsync(userDto.Email) is not null) continue;

                // Create user in Identity
                var user = mapper.Map<ApplicationUser>(userDto);
                user.EmailConfirmed = true;
                var result = await userManager.CreateAsync(user, userDto.Password);
                if (!result.Succeeded) continue;

                // Assign proper role
                var roleName = userDto.User_Type.ToString();
                if (await roleManager.RoleExistsAsync(roleName))
                    await userManager.AddToRoleAsync(user, roleName);

                // Recruiter profile
                if (userDto.User_Type == UserType.Recruiter)
                {
                    var RecruiterDto = Recruiters.FirstOrDefault(e => e.UserEmail == user.Email);
                    if (RecruiterDto != null)
                    {
                        var Recruiter = mapper.Map<RecruiterProfile>(RecruiterDto);
                        Recruiter.UserId = user.Id;
                        context.RecruiterProfiles.Add(Recruiter);
                        await context.SaveChangesAsync();
                    }
                }
                // Candidate profile
                else if (userDto.User_Type == UserType.Candidate)
                {
                    var CandidateDto = Candidates.FirstOrDefault(s => s.UserEmail == user.Email);
                    if (CandidateDto != null)
                    {
                        try
                        {
                            var Candidate = mapper.Map<CandidateProfile>(CandidateDto);
                            Candidate.UserId = user.Id;

                            // Skills – match IDs from JSON to DB
                            if (CandidateDto.Skills?.Any() == true)
                                Candidate.Skills = await context.Skills.Where(s => CandidateDto.Skills.Contains(s.Id)).ToListAsync();

                            // Certificates, Trainings, Interests, Experiences, Educations
                            if (CandidateDto.CandidateCertificates?.Any() == true)
                                Candidate.CandidateCertificates = mapper.Map<List<CandidateCertificate>>(CandidateDto.CandidateCertificates);
                            if (CandidateDto.CandidateTraining?.Any() == true)
                                Candidate.CandidateTraining = mapper.Map<List<CandidateTraining>>(CandidateDto.CandidateTraining);
                            if (CandidateDto.CandidateInterests?.Any() == true)
                                Candidate.CandidateInterests = mapper.Map<List<CandidateInterest>>(CandidateDto.CandidateInterests);
                            if (CandidateDto.CandidateExperiences?.Any() == true)
                                Candidate.CandidateExperiences = mapper.Map<List<CandidateExperience>>(CandidateDto.CandidateExperiences);
                            if (CandidateDto.CandidateEducations?.Any() == true)
                                Candidate.CandidateEducations = mapper.Map<List<CandidateEducation>>(CandidateDto.CandidateEducations);

                            context.CandidateProfiles.Add(Candidate);
                            await context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                            throw;
                        }
                    }
                }
            }
        }

        // ================= Applications =================
        private static async Task SeedApplicationsAsync(ApplicationDbContext context, IMapper mapper)
        {
            if (await context.CandidateApplications.AnyAsync()) return;

            var applicationDtos = await LoadJsonFileAsync<ApplicationSeedDto>("applications.json");
            if (applicationDtos == null || !applicationDtos.Any()) return;

            var allApplications = new List<CandidateApplication>();

            // Get all Candidates and jobs for validation
            var allCandidates = await context.CandidateProfiles.ToListAsync();
            var allJobs = await context.Jobs.ToListAsync();

            foreach (var userDto in applicationDtos)
            {
                // Validate if ApplicantId exists
                var CandidateExists = allCandidates.Any(s => s.Id == userDto.ApplicantId);
                if (!CandidateExists)
                {
                    Console.WriteLine($"Skipping applications for ApplicantId {userDto.ApplicantId}: Candidate not found");
                    continue;
                }

                foreach (var applicationDto in userDto.Applications)
                {
                    // Validate if jobID exists
                    var jobExists = allJobs.Any(j => j.Id == applicationDto.JobId);
                    if (!jobExists)
                    {
                        Console.WriteLine($"Skipping application: JobId {applicationDto.JobId} not found");
                        continue;
                    }

                    // Use AutoMapper to map combined data
                    var application = mapper.Map<CandidateApplication>((userDto, applicationDto));
                    allApplications.Add(application);
                }
            }

            if (allApplications.Any())
            {
                await context.CandidateApplications.AddRangeAsync(allApplications);
                await context.SaveChangesAsync();
            }
        }
        // ================= Helpers =================
        private static async Task<List<T>> LoadJsonFileAsync<T>(string fileName)
        {
            var path = Path.Combine(DataPath, fileName);
            var json = await File.ReadAllTextAsync(path);
            return JsonSerializer.Deserialize<List<T>>(json, JsonOptions);
        }

        private static async Task SeedDataAsync<T>(ApplicationDbContext context, DbSet<T> dbSet, string fileName) where T : class
        {
            if (await dbSet.AnyAsync()) return;

            var items = await LoadJsonFileAsync<T>(fileName);
            if (items == null || !items.Any()) return;

            await dbSet.AddRangeAsync(items);
            await context.SaveChangesAsync();
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var roles = new[] { "Admin", "Recruiter", "Candidate" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

}
