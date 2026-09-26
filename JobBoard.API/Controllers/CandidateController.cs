using JobBoard.Application.DTOs.AuthDTOs;
using JobBoard.Application.DTOs.CandidateDTOs;
using JobBoard.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoard.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CandidateController : ControllerBase
	{
		private readonly ICandidateService CandidateService;
        private string? userId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public CandidateController(ICandidateService CandidateService)
        {
            this.CandidateService = CandidateService;
        }

        /*------------------------ Get All Profiles --------------------------*/
        [HttpGet]
		[Authorize(Roles = "Admin")]

		public async Task<IActionResult> GetAll()
        {
            var Candidates = await CandidateService.GetAllAsync();
            return Ok(Candidates);
        }

        /*------------------------ Get My Profile --------------------------*/
        [HttpGet("GetMyProfile")]
		[Authorize(Roles = "Candidate")]
		public async Task<IActionResult> GetMyProfile()
        {
            if (userId == null)
                return Unauthorized(new ResultDto(false, "User not authenticated"));

            var CandidateProfile = await CandidateService.GetByUserIdAsync(userId);

            if (CandidateProfile == null)
                return NotFound(new ResultDto(false, "Candidate profile not found"));

            return Ok(CandidateProfile);
        }

        /*------------------------ Update Profile --------------------------*/
        [HttpPut]
		[Authorize(Roles = "Candidate")]
		public async Task<IActionResult> Update([FromBody] CandidateProfileUpdateDto dto)
        {
            if (userId == null)
                return Unauthorized(new ResultDto(false, "User not authenticated"));

            var existingProfile = await CandidateService.GetByUserIdAsync(userId);
            if (existingProfile == null)
                return NotFound(new ResultDto(false, "Candidate profile not found"));

            var result = await CandidateService.UpdateAsync(userId , dto);
            if (!result)
                return BadRequest(result);

            return Ok(result);
        }


		/*------------------------ Upload Files (Profile Image & CV) --------------------------*/
		[HttpPost("upload-files")]
		[Authorize(Roles = "Candidate")]
		public async Task<IActionResult> UploadFiles([FromForm] CandidateFileUploadDto dto)
		{
			if (userId == null)
				return Unauthorized(new ResultDto(false, "User not authenticated"));

			var result = await CandidateService.UploadFilesAsync(userId, dto);

			if (result.CvUrl == null && result.ProfileImageUrl == null)
				return NotFound(new ResultDto(false, "Candidate profile not found"));

			return Ok(new
			{
				CV_Url = result.CvUrl,
				ProfileImageUrl = result.ProfileImageUrl
			});
		}

		/*------------------------ Delete Profile by Id --------------------------*/
		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteById(int id)
        {
            var result = await CandidateService.DeleteAsync(id);
            if (!result)
                return NotFound(result);

            return Ok(result);
        }
    }
}