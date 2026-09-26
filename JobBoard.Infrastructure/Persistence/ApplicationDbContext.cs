using JobBoard.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace JobBoard.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<CandidateProfile> CandidateProfiles { get; set; }
        public DbSet<CandidateEducation> CandidateEducations { get; set; }
        public DbSet<CandidateExperience> CandidateExperiences { get; set; }
        public DbSet<CandidateCertificate> CandidateCertificates { get; set; }
        public DbSet<CandidateInterest> CandidateInterests { get; set; }
        public DbSet<CandidateTraining> CandidateTrainings { get; set; }
        public DbSet<RecruiterProfile> RecruiterProfiles { get; set; }
        public DbSet<CandidateApplication> CandidateApplications { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SavedJob> SavedJobs { get; set; }
        public DbSet<AIEmbedding> AIEmbeddings { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}

