using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Specifications.CandidateSpecifications
{
    public class CandidateSkillsSpecification : BaseSpecifications<Skill> 
    { 
        public CandidateSkillsSpecification(List<string> skillNames) : 
            base(s => skillNames.Contains(s.SkillName)) { }
    }
}
