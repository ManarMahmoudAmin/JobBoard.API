using JobBoard.Application.DTOs.CandidateApplicationDTOs;
using JobBoard.Application.Features.CandidatApplications.Commands.CreateApplication;
using JobBoard.Application.Features.CandidatApplications.Commands.DeleteApplication;
using JobBoard.Application.Features.CandidatApplications.Commands.UpdateApplicationStatus;
using JobBoard.Application.Features.CandidatApplications.Query.GetApplicationById;
using JobBoard.Application.Features.CandidatApplications.Query.GetApplicationsByApplicantId;
using JobBoard.Application.Features.CandidatApplications.Query.GetApplicationsByJobId;
using JobBoard.Application.Features.CandidatApplications.Query.GetApplicationsForRecruiterJobs;
using JobBoard.Application.Features.CandidatApplications.Query.HasUserAppliedToJob;
using JobBoard.Application.Features.Candidate.Query.GetByUserId;
using JobBoard.Application.Features.Recruiter.Query.GetByUserIdForRecruiter;
using JobBoard.Application.Interfaces;
using JobBoard.Application.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoard.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ApplicationController : ControllerBase
	{
		//private readonly IApplicationService _applicationService;
		//private readonly ICandidateService _candidateService;
		//private readonly IRecruiterService _recruiterService;
		private readonly IMediator _mediator;

		private string? userId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		public ApplicationController(IMediator mediator)
		{
			_mediator = mediator;
		}

		//--------------------------Candidate----------------------
		/// Create a new job application (Candidate only)
		// POST: api/application
		[HttpPost]
		[Consumes("multipart/form-data")]
		[Authorize(Roles = "Candidate")]
		public async Task<IActionResult> CreateApplication([FromForm] CreateApplicationDto createDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (createDto.JobId <= 0)
				return BadRequest("Invalid Job Id.");

			if (userId == null)
				return Unauthorized();

			//var CandidateProfile = await _candidateService.GetByUserIdAsync(userId);
			var CandidateProfile = await _mediator.Send(new GetByUserIdQuery(userId));

            if (CandidateProfile == null)
				return Unauthorized("Candidate profile not found");

			//var createdApplication = await _applicationService.CreateApplicationAsync(createDto, CandidateProfile.Id);
			var createdApplication = await _mediator.
				Send(new CreateApplicationCommand(createDto, CandidateProfile.Id));


            if (createdApplication == null)
				return BadRequest("Unable to create application. You may have already applied to this job or the job doesn't exist.");

			return CreatedAtAction(nameof(GetApplicationById), new { id = createdApplication.Id }, createdApplication);
		}

		/// Get all applications submitted by the current Candidate
		// GET: api/application/my-applications
		[HttpGet("my-applications")]
		[Authorize(Roles = "Candidate")]
		public async Task<ActionResult<IEnumerable<CandidateApplicationListDto>>> GetMyApplications()
		{
			if (userId == null)
				return Unauthorized();

            var CandidateProfile = await _mediator.Send(new GetByUserIdQuery(userId));
            if (CandidateProfile == null)
				return Unauthorized("Candidate profile not found");

			//var applications = await _applicationService.GetApplicationsByApplicantIdAsync(CandidateProfile.Id);
			var applications = await _mediator.Send(new GetApplicationsByApplicantIdQuery()
			{
				ApplicantId = CandidateProfile.Id
			});

            return Ok(applications);
		}

		/// Checks if the current Candidate has already applied to a specific job
		// GET: api/application/has-applied/{jobId}
		[HttpGet("has-applied/{jobId:int}")]
		[Authorize(Roles = "Candidate")]
		public async Task<ActionResult<bool>> HasApplied(int jobId)
		{
			if (jobId <= 0)
				return BadRequest("Invalid JobId.");

			if (userId == null)
				return Unauthorized();

            var CandidateProfile = await _mediator.Send(new GetByUserIdQuery(userId));
            if (CandidateProfile == null)
				return Unauthorized("Candidate profile not found");

			//var hasApplied = await _applicationService.
			//	HasUserAppliedToJobAsync(CandidateProfile.Id, jobId);
			var hasApplied = await _mediator.Send(new HasUserAppliedToJobQuery()
			{
				ApplicantId = CandidateProfile.Id,
				JobId = jobId
            });

            return Ok(hasApplied);
		}

		//--------------------------Recruiter----------------------

		/// Get all applications for jobs posted by the current Recruiter
		// GET: api/application/Recruiter-applications
		[HttpGet("Recruiter-applications")]
		[Authorize(Roles = "Recruiter")]
		public async Task<ActionResult<IEnumerable<RecruiterApplicationListDto>>>
			GetRecruiterApplications([FromQuery] ApplicationFilterParams filterParams)
		{
			if (userId == null)
				return Unauthorized();

            //var RecruiterProfile = await _recruiterService.GetByUserId(userId);
            var RecruiterProfile = await _mediator.Send(new GetByUserIdForRecruiterQuery()
			{
				UserId = userId
			});

            if (RecruiterProfile == null)
				return Unauthorized("Recruiter profile not found");

			//var applications = await _applicationService.GetApplicationsForRecruiterJobsAsync(RecruiterProfile.Id, filterParams);
			var applications = await _mediator.Send(new GetApplicationsForRecruiterJobsQuery(RecruiterProfile.Id, filterParams));

            return Ok(applications);
		}

		/// Get all applications for a specific job posted by the current Recruiter
		// GET: api/job-applications/{id}
		[HttpGet("job-applications/{jobId:int}")]
		[Authorize(Roles = "Recruiter")]
		public async Task<ActionResult<IEnumerable<RecruiterApplicationListDto>>> GetApplicationsByJobId(int jobId)
		{
			if (jobId <= 0)
				return BadRequest("Invalid Job Id.");

			if (userId == null)
				return Unauthorized();

            var RecruiterProfile = await _mediator.Send(new GetByUserIdForRecruiterQuery()
            {
                UserId = userId
            }); 
			if (RecruiterProfile == null)
				return Unauthorized("Recruiter profile not found");

			//var applications = await _applicationService.GetApplicationsByJobIdAsync(jobId, RecruiterProfile.Id);

			var applications = await _mediator.Send(new GetApplicationsByJobIdQuery()
			{
				JobId = jobId,
				RecruiterId = RecruiterProfile.Id
            });

            return Ok(applications);
		}
		/// Update the status of an application 
		// PUT: api/application/status/{id}
		[HttpPut("Status/{id:int}")]
		[Authorize(Roles = "Recruiter")]
		public async Task<IActionResult> UpdateApplicationStatus(int id, [FromBody] UpdateApplicationStatusDto statusDto)
		{
			if (id <= 0)
				return BadRequest("Invalid application Id.");

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (userId == null)
				return Unauthorized();

            var RecruiterProfile = await _mediator.Send(new GetByUserIdForRecruiterQuery()
            {
                UserId = userId
            }); 
			if (RecruiterProfile == null)
				return Unauthorized("Recruiter profile not found");

			//var updated = await _applicationService.UpdateApplicationStatusAsync(id, statusDto.Status, RecruiterProfile.Id);
			var updatedApplication = await _mediator.Send(new UpdateApplicationStatusCommand()
			{
				ApplicationId = id,
				RecruiterId = RecruiterProfile.Id,
				Status = statusDto.Status
			});

            if (!updatedApplication)
				return NotFound("Application not found or you don't have permission to update it.");

			return NoContent();
		}


		//--------------------------Admin----------------------
		/// Delete an application (Admin only)
		// DELETE: api/application/{id}
		[HttpDelete("{id:int}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteApplication(int id)
		{
			if (id <= 0)
				return BadRequest("Invalid application Id.");

			//var deleted = await _applicationService.DeleteApplicationAsync(id);
			var deletedApplication = await _mediator.Send(new DeleteApplicationCommand(id));
            if (!deletedApplication)
				return NotFound("Application not found.");

			return NoContent();
		}


		//-------------------------- Mutual endpoint (Shared across roles)----------------------
		/// Gets a specific application by ID
		// GET: api/application/{id}
		[HttpGet("{id:int}")]
		[Authorize]
		public async Task<ActionResult<ApplicationDto>> GetApplicationById(int id)
		{
			if (id <= 0)
				return BadRequest("Invalid application Id.");

			//var application = await _applicationService.GetApplicationByIdAsync(id);
			var application = await _mediator.Send(new GetApplicationByIdQuery()
			{
				Id = id
			});
			if (application == null)
				return NotFound("Application not found.");

			// Check authorization - only the applicant, job owner, or admin can view
			if (User.IsInRole("Admin"))
				return Ok(application);

			if (User.IsInRole("Candidate"))
			{
                var CandidateProfile = await _mediator.Send(new GetByUserIdQuery(userId));
                if (CandidateProfile != null && application.ApplicantId == CandidateProfile.Id)
					return Ok(application);
			}

			if (User.IsInRole("Recruiter"))
			{
                var RecruiterProfile = await _mediator.Send(new GetByUserIdForRecruiterQuery()
                {
                    UserId = userId
                }); 
				if (RecruiterProfile != null && application.Job?.RecruiterId == RecruiterProfile.Id)
					return Ok(application);
			}

			return Forbid("You don't have permission to view this application.");
		}

	}
}