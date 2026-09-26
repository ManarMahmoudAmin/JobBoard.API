using JobBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Infrastructure.Data.Configurations
{
	class RecruiterProfileConfigurations : IEntityTypeConfiguration<RecruiterProfile>
	{
		public void Configure(EntityTypeBuilder<RecruiterProfile> builder)
		{
			builder.HasKey(e => e.Id);

			builder.HasOne(e => e.User)
			   .WithOne(u=>u.RecruiterProfile)
			   .HasForeignKey<RecruiterProfile>(e => e.UserId)
			   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.IsDeleted).HasDefaultValue(false);
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
