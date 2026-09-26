namespace JobBoard.Application.DTOs.ExternalLoginDTOs
{
    public class ExternalLoginRequestDto
    {
        public string IdToken { get; set; }
        public string Role { get; set; }  // Candidate or Recruiter
    }
}
