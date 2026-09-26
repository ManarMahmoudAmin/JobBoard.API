using JobBoard.Domain.Entities;

namespace JobBoard.Application.Specifications.JobSpecifications
{
    public class CategoriesByIdsSpecifications : BaseSpecifications<Category>
    {
		public CategoriesByIdsSpecifications(IEnumerable<int> ids)
		: base(c => ids.Contains(c.Id))
		{
		}
	}
}
