using JobBoard.Domain.Enums;

namespace JobBoard.Application.Shared
{
    public static class ApplicationStatusHelper
	{

		public static string GetStatusDisplay(ApplicationStatus status)
		{
			switch (status)
			{
				case ApplicationStatus.Pending:
					return "New";
				case ApplicationStatus.UnderReview:
					return "Under Review";
				case ApplicationStatus.Interviewed:
					return "Interview";
				case ApplicationStatus.Accepted:
					return "Hired";
				case ApplicationStatus.Rejected:
					return "Rejected";
				default:
					return "Unknown";
			}
		}

	};


}