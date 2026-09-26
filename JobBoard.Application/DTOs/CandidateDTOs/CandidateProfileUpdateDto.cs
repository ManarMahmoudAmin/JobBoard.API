namespace JobBoard.Application.DTOs.CandidateDTOs
{
    using JobBoard.Domain.Enums;
    using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public class CandidateProfileUpdateDto
	{
		[Required(ErrorMessage = "Name is required")]
		[StringLength(100, ErrorMessage = "Name must be less than 100 characters")]
		public string? Name { get; set; }

		[Phone(ErrorMessage = "Invalid phone number format")]
		[StringLength(20, ErrorMessage = "Phone number must be less than 20 characters")]
		public string? PhoneNumber { get; set; }

		[StringLength(100, ErrorMessage = "Title must be less than 100 characters")]
		public string? Title { get; set; }

		public string? Address { get; set; }

		public DateTime? DateOfBirth { get; set; }

		public Gender? Gender { get; set; }

		public string? Summary { get; set; }

		// Skills
		public List<string>? Skills { get; set; }

		// Interests
		public List<string>? Interests { get; set; }

		// Certificates
		public List<string>? Certificates { get; set; }

		// Trainings
		public List<string>? Trainings { get; set; }

		// Educations
		public List<CandidateEducationUpdateDto>? CandidateEducations { get; set; }

		// Experiences
		public List<CandidateExperienceUpdateDto>? CandidateExperiences { get; set; }
	}

}
