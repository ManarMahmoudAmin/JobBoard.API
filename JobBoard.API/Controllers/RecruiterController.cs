using JobBoard.Application.DTOs.RecruiterDTOs;
using JobBoard.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class RecruiterController : ControllerBase
    {
        private string? userId => User.FindFirstValue(ClaimTypes.NameIdentifier);

		private readonly IRecruiterService RecruiterService;

		public RecruiterController(IRecruiterService RecruiterService)
		{
			this.RecruiterService = RecruiterService;
		}


        /*------------------------Get All Profiles --------------------------*/
        [HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetAll()
        {      
            if (userId == null)
                return Unauthorized();
            var result = await RecruiterService.GetAll();
            if (result == null || !result.Any())
                return NotFound("No Recruiter profiles found");
            return Ok(result);
        }


        /*------------------------Get my Profile --------------------------*/
        [HttpGet("my-profile")]
		[Authorize(Roles = "Recruiter")]
		public async Task<IActionResult> GetById()
        {
            if (userId == null)
                return Unauthorized();

			var result = await RecruiterService.GetByUserId(userId);

			if (result == null)
				return NotFound();

			return Ok(result);
		}


		/*------------------------create --------------------------*/
		//[HttpPost]
		//public async Task<IActionResult> Create([FromBody] RecruiterProfileDto dto)
		//{
		//    if (userId == null)
		//        return Unauthorized();

		//    var existingProfile = await RecruiterService.GetByUserId(userId);
		//    if (existingProfile != null)
		//        return BadRequest("Recruiter profile already exists");



		/*------------------------Update --------------------------*/
		[HttpPut]
		[Consumes("multipart/form-data")]
		[Authorize(Roles = "Recruiter")]
		public async Task<IActionResult> Update([FromForm] RecruiterProfileUpdateDto dto)
		{
			if (userId == null)
				return Unauthorized();

			var updatedProfile = await RecruiterService.GetByUserId(userId);
			if (updatedProfile == null)
				return NotFound();

			bool updated = await RecruiterService.Update(updatedProfile.Id, dto);

			return updated ? Ok("updated") : NotFound();
		}

		/*------------------------Delete --------------------------*/
		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete([FromRoute] int id)
		{
			if (userId == null)
				return Unauthorized();

			var deletedProfile = await RecruiterService.GetByUserId(userId);
			if (deletedProfile == null)
				return NotFound();

			bool deleted = await RecruiterService.DeleteById(deletedProfile.Id);
			return deleted ? Ok("Deleted") : BadRequest();
		}
    }
}
