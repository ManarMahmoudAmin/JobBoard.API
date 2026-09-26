using JobBoard.Application.Shared.SortingOptions;

namespace JobBoard.Application.Shared
{
	public class SavedJobFilterParams
	{
		public string? SearchValue { get; set; }
		public SortingDateOptions SortingOption { get; set; } = SortingDateOptions.DateDesc;
		public int? CandidateId { get; set; }
	}
}
