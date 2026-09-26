using JobBoard.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobBoard.Application.DTOs.CandidateApplicationDTOs

{
    public class UpdateApplicationStatusDto
    {
		[Required]
		[EnumDataType(typeof(ApplicationStatus))]
		public ApplicationStatus Status { get; set; }
	}

}
