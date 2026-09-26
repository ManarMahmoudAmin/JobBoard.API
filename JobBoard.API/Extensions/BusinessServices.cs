using JobBoard.API.Hubs;
using JobBoard.Application.Interfaces;
using JobBoard.Application.Services;
using JobBoard.Infrastructure;
using JobBoard.Infrastructure.Services;

namespace JobBoard.API.Extensions
{
    public static class BusinessServices
	{
		public static IServiceCollection AddBusinessServices(this IServiceCollection services)
		{
			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<IRecruiterService, RecruiterService>();
			services.AddScoped<ICandidateService, CandidateService>();
			services.AddScoped<IJobService, JobService>();
			services.AddScoped<IEmailService, EmailService>();
			services.AddScoped<IApplicationService, ApplicationService>();
			services.AddScoped<ISavedJobService, SavedJobService>();
			services.AddScoped<IAdminService, AdminService>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<IFileService, FileService>();
			services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationSender, SignalRNotificationSender>();
            services.AddSignalR();
            return services;
		}
	}
}