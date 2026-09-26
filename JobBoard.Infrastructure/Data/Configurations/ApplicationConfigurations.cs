using JobBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Infrastructure.Data.Configurations
{
	class ApplicationConfigurations : IEntityTypeConfiguration<CandidateApplication>
	{
		public void Configure(EntityTypeBuilder<CandidateApplication> builder)
		{
			builder.HasOne(a => a.Job)
				.WithMany(j => j.JobApplications)
				.HasForeignKey(a => a.JobId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(a => a.Applicant)
				.WithMany(s => s.UserApplications)
				.HasForeignKey(a => a.ApplicantId)
				.OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new
            {
                x.ApplicantId,
                x.JobId
            }).IsUnique();
        }
	}
}
