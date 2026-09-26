using JobBoard.Application.DTOs.SavedJobDTOs;
using JobBoard.Application.Interfaces;
using JobBoard.Application.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoard.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Candidate")]
	public class SavedJobController : ControllerBase
	{
		private string? userId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		private readonly ISavedJobService _savedJobService;
		private readonly ICandidateService _CandidateService;

		public SavedJobController(ISavedJobService savedJobService, ICandidateService CandidateService)
		{
			_savedJobService = savedJobService;
			_CandidateService = CandidateService;
		}


		// GET: api/SavedJob
		[HttpGet]
		[HttpGet]
		public async Task<IActionResult> GetAllSavedJobs([FromQuery] SavedJobFilterParams filterParams)
		{
			var CandidateId = await GetCandidateIdAsync();
			if (CandidateId.Result != null)
				return CandidateId.Result;

			var result = await _savedJobService.GetSavedJobsAsync(CandidateId.Value, filterParams);
			return Ok(result);
		}

		// GET: api/SavedJob/{jobId}
		[HttpGet("{jobId:int}")]
		public async Task<IActionResult> GetSavedJob(int jobId)
		{
			var CandidateId = await GetCandidateIdAsync();
			if (CandidateId.Result != null)
				return CandidateId.Result;

			var savedJob = await _savedJobService.GetSavedJobByIdAsync(CandidateId.Value, jobId);
			if (savedJob == null)
				return NotFound();
			return Ok(savedJob);
		}

		// POST: api/SavedJob
		[HttpPost]
		public async Task<IActionResult> SaveJob([FromBody] CreateSavedJobDto savedJobDto)
		{
			var CandidateId = await GetCandidateIdAsync();
			if (CandidateId.Result != null)
				return CandidateId.Result;

			var savedJob = await _savedJobService.SaveJobAsync(CandidateId.Value, savedJobDto);
			if (savedJob == null)
				return BadRequest("Job is already saved or does not exist");

			return Ok(savedJob);
		}

		// GET: api/SavedJob/issaved/{jobId}
		[HttpGet("issaved/{jobId:int}")]
		public async Task<IActionResult> IsJobSaved(int jobId)
		{
			var CandidateId = await GetCandidateIdAsync();
			if (CandidateId.Result != null)
				return CandidateId.Result;

			var isSaved = await _savedJobService.IsJobSavedAsync(CandidateId.Value, jobId);
			return Ok(isSaved);
		}

		// DELETE: api/SavedJob/{jobId}
		[HttpDelete("{jobId:int}")]
		public async Task<IActionResult> UnsaveJob(int jobId)
		{
			var CandidateId = await GetCandidateIdAsync();
			if (CandidateId.Result != null)
				return CandidateId.Result;

			var deleted = await _savedJobService.UnsaveJobAsync(CandidateId.Value, jobId);
			if (!deleted)
				return NotFound();

			return NoContent();
		}

		// method to get CandidateId or return Unauthorized
		private async Task<ActionResult<int>> GetCandidateIdAsync()
		{
			if (userId == null)
				return Unauthorized();

			var Candidate = await _CandidateService.GetByUserIdAsync(userId);
			if (Candidate == null)
				return Unauthorized("Candidate profile not found");

			return Candidate.Id;
		}
	}
}
