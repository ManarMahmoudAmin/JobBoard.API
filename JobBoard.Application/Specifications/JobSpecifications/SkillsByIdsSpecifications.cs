using JobBoard.Domain.Entities;

namespace JobBoard.Application.Specifications.JobSpecifications
{
    public class SkillsByIdsSpecifications : BaseSpecifications<Skill>
	{
		public SkillsByIdsSpecifications(IEnumerable<int> ids)
			: base(s => ids.Contains(s.Id))
		{
		}
	}
	
}
