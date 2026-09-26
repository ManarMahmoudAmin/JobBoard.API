using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Specifications.RecruiterSpecifications
{
    public class RecruiterForUpdateSpecification : BaseSpecifications<RecruiterProfile>
    {
        public RecruiterForUpdateSpecification(int id) : base(r => r.Id == id)
        {
            AddIncludes(r => r.User);
        }
    }
}
