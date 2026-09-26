using JobBoard.Application.Shared.SortingOptions;

namespace JobBoard.Application.Shared
{
    public class RecruiterJobFilterParams
    {
		public RecruiterJobStatus? Status { get; set; }
		public RecruiterJobSortingOptions SortingOption { get; set; } = RecruiterJobSortingOptions.PostedDateDesc;
		public string? SearchValue { get; set; }
	}

	public enum RecruiterJobStatus
	{
		Active, Filled, Expired
	}
}
