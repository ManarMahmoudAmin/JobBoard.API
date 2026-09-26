using JobBoard.Application.Attributes;
using Microsoft.AspNetCore.Http;

namespace JobBoard.Application.DTOs.CandidateDTOs
{
    public class CandidateFileUploadDto
    {
		[AllowedExtensions("jpg", "jpeg", "png", "gif", "webp", ErrorMessage = "Only image files are allowed.")]
		public IFormFile? ProfileImageUrl { get; set; }

		[AllowedExtensions("pdf", "doc", "docx", ErrorMessage = "Only PDF and Word documents are allowed for CV.")]
		public IFormFile? CV_Url { get; set; }
		public bool RemoveProfileImage { get; set; } = false;
		public bool RemoveCV { get; set; } = false;
	}
}
