using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Specifications.DashboardSpecifications
{
    public class JobsExpiringSoonSpecification : BaseSpecifications<Job>
    {
        public JobsExpiringSoonSpecification(int RecruiterId, DateTime expiringDate)
            : base(j => j.RecruiterId == RecruiterId &&
                        j.IsApproved && j.IsActive &&
                        j.ExpireDate <= expiringDate &&
                        j.ExpireDate.Value >= DateTime.Now)
        {


        }
    }
}
