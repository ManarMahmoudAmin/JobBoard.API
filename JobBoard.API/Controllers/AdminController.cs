
using JobBoard.API.Extensions;
using JobBoard.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }


        /////////////////////////get all Candidates///////////////////////
        [HttpGet("Candidates")]
        [Cached(600, "admin:")]
        public async Task<IActionResult> GetCandidates()
        {
            var Candidates = await _adminService.GetAllCandidatesAsync();
            if (Candidates == null) 
            {
                return NotFound();
            }
            return Ok(Candidates);
        }

        /////////////////////////get Candidate by id///////////////////////
        
        [HttpGet("Candidate/{CandidateId}")]
        public async Task<IActionResult> GetCandidateById(string CandidateId)
        {
            var Candidate = await _adminService.GetCandidateByIdAsync(CandidateId);
            if (Candidate == null)
            {
                return NotFound();
            }
            return Ok(Candidate);
        }


        /////////////////////////get all Recruiters//////////////////////
        [HttpGet("Recruiters")]
        [Cached(600, "admin:")]
        public async Task<IActionResult> GetRecruiters()
        {
            var Recruiters = await _adminService.GetAllRecruitersAsync();
            if (Recruiters == null) 
            {
                return NotFound();
            }
            return Ok(Recruiters);
        }

        /////////////////////////get Recruiter by id///////////////////////
        [HttpGet("Recruiter/{RecruiterId}")]
        public async Task<IActionResult> GetRecruiterById(string RecruiterId)
        {
            var Recruiter = await _adminService.GetRecruiterByIdAsync(RecruiterId);
            if (Recruiter == null)
            {
                return NotFound();
            }
            return Ok(Recruiter);
        }


        /// //////////////////delete user by id///////////////////////
        [HttpDelete("user/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _adminService.DeleteUserAsync(userId);
            if (!result) return NotFound("User not found.");
            return Ok("User deleted successfully.");
        }


        ////////////////////////get all jobs///////////////////////
        [HttpGet("jobs")]
        public async Task<IActionResult> GetAllJobs()
        {
            var jobs = await _adminService.GetAllJobsAsync();
            if (jobs == null)
            {
                return NotFound();
            }
            return Ok(jobs);
        }


        ////////////////////////pending jobs///////////////////////
        [HttpGet("jobs/pending")]
        public async Task<IActionResult> GetPendingJobs()
        {
            var jobs = await _adminService.GetPendingJobsAsync();
            if(jobs == null)
            {
                return NotFound();
            }
            return Ok(jobs);
        }


        // //////////////////approve job by id///////////////////////
        [HttpPut("jobs/{jobId}/approve")]
        public async Task<IActionResult> ApproveJob(int jobId)
        {
            var result = await _adminService.ApproveJobAsync(jobId);
            if (!result) 
                return NotFound("Job not found or already approved.");
            
            return Ok("Job approved successfully.");
        }



        // //////////////////reject job by id///////////////////////
        [HttpDelete("jobs/{jobId}/reject")]
        public async Task<IActionResult> RejectJob(int jobId)
        {
            var result = await _adminService.RejectJobAsync(jobId);
            if (!result) return NotFound("Job not found.");
            return Ok("Job rejected successfully.");
        }



        ////////////////////////get stats///////////////////////
        
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var stats = await _adminService.GetStatsAsync();
            return Ok(stats);
        }
	}

}
