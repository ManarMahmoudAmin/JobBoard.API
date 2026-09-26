using JobBoard.Application.Attributes;
using JobBoard.Domain.Enums;
using System.ComponentModel.DataAnnotations;


namespace JobBoard.Application.DTOs.JobDTOs
{
    [TeamSizeValidation]
	public class CreateUpdateJobDto
	{
		[Required]
		[StringLength(200)]
		public string Title { get; set; }
		public string Description { get; set; }


		[Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive number")]
		public decimal? Salary { get; set; }
		public DateTime? ExpireDate { get; set; }

		[Required]
		public WorkplaceType WorkplaceType { get; set; }

		[Required]
		public JobType JobType { get; set; }
		public EducationLevel? EducationLevel { get; set; }
		public ExperienceLevel? ExperienceLevel { get; set; }

		[Range(1, 100)]
		public int? MinTeamSize { get; set; }

		[Range(1, 100)]
		public int? MaxTeamSize { get; set; }
		public bool IsActive { get; set; } = true;
		public string? Requirements { get; set; }
		public string? Responsabilities { get; set; }
		public string? Benefits { get; set; }
		public List<int> CategoryIds { get; set; } = [];
		public List<int> SkillIds { get; set; } = [];
	}
}
