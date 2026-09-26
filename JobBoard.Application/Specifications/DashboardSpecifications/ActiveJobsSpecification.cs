using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Specifications.DashboardSpecifications
{
    public class ActiveJobsSpecification : BaseSpecifications<Job>
    {
        public ActiveJobsSpecification(int RecruiterId)
            : base(j => j.RecruiterId == RecruiterId &&
                        j.IsApproved &&
                        j.IsActive &&
                      (!j.ExpireDate.HasValue || j.ExpireDate > DateTime.Now))
        {
        }
    }
}
