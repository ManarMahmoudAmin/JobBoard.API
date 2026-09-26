using JobBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Infrastructure.Data.Configurations
{
	class CandidateProfileConfigurations : IEntityTypeConfiguration<CandidateProfile>
	{
		public void Configure(EntityTypeBuilder<CandidateProfile> builder)
		{
			builder.HasKey(s => s.Id);

            // string length limits and nullable
            builder.Property(s => s.Name).HasMaxLength(200).IsRequired(false);
            builder.Property(s => s.Title).HasMaxLength(200).IsRequired(false);
            builder.Property(s => s.Address).HasMaxLength(500).IsRequired(false);
            builder.Property(s => s.Summary).HasMaxLength(2000).IsRequired(false);
            builder.Property(s => s.CV_Url).HasMaxLength(1000).IsRequired(false);
            builder.Property(s => s.ProfileImageUrl).HasMaxLength(1000).IsRequired(false);
            builder.Property(s => s.Gender).HasConversion<string>();


            // FK to Identity user (required and unique because one-to-one)
            builder.Property(s => s.UserId)
                   .IsRequired();

            builder.HasIndex(s => s.UserId)
                   .IsUnique();

            builder.HasOne(s => s.User)
					.WithOne(u=>u.CandidateProfile)
					.HasForeignKey<CandidateProfile>(s => s.UserId)
					.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(s => s.Skills)
				    .WithMany(sk => sk.Candidates)
					.UsingEntity(j => j.ToTable("CandidateSkills"));

            // experiences (one-to-many)
            builder.HasMany(s => s.CandidateExperiences)
                   .WithOne(e => e.CandidateProfile)
                   .HasForeignKey(e => e.CandidateProfileId)
                   .OnDelete(DeleteBehavior.Cascade);

            // education (one-to-many)
            builder.HasMany(s => s.CandidateEducations)
                   .WithOne(e => e.CandidateProfile)
                   .HasForeignKey(e => e.CandidateProfileId)
                   .OnDelete(DeleteBehavior.Cascade);

            //intersts (one-to-many)
            builder.HasMany(s=> s.CandidateInterests)
                .WithOne(i => i.CandidateProfile)
                .HasForeignKey(i => i.CandidateProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            //trainings (one-to-many)
            builder.HasMany(s => s.CandidateTraining)
                .WithOne(t => t.CandidateProfile)
                .HasForeignKey(t => t.CandidateProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            //certificates  (one-to-many)
            builder.HasMany(s => s.CandidateCertificates)
                .WithOne(c => c.CandidateProfile)
                .HasForeignKey(c => c.CandidateProfileId)
                .OnDelete(DeleteBehavior.Cascade);

        }
	}
}
