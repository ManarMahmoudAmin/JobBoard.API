using JobBoard.Application.Shared.SortingOptions;
using JobBoard.Domain.Enums;

namespace JobBoard.Application.Shared
{
    public class JobFilterParams
    {
		public int? CategoryId { get; set; } 
		public int? SkillId { get; set; }
		public int? RecruiterId { get; set; }
		public WorkplaceType? WorkplaceType { get; set; }
		public JobType? JobType { get; set; }
		public ExperienceLevel? ExperienceLevel { get; set; }
		public EducationLevel? EducationLevel { get; set; }
		public bool? IsActive { get; set; }
		public JobSortingOptions SortingOption { get; set; }
		public string? SearchValue { get; set; }
		public string? SearchByLocationValue { get; set; }
		private int _pageIndex = 1;
		public int PageIndex
		{
			get => _pageIndex;
			set => _pageIndex = (value < 1) ? 1 : value;
		}

		private int _pageSize = 6;
		public int PageSize
		{
			get => _pageSize;
			set => _pageSize = (value > 100) ? 100 : (value < 1 ? 1 : value); 
		}
	}
}
