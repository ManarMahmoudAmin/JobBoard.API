namespace JobBoard.Application.DTOs.AdminDTOs

{
    public class CandidateListDto
    {
		public int Id { get; set; }
		public string UserId { get; set; } = string.Empty;
		public string? Name { get; set; }
		public string? Email { get; set; }
		public string? PhoneNumber { get; set; }
		public List<string>? SkillName { get; set; } = new();

	}
}
