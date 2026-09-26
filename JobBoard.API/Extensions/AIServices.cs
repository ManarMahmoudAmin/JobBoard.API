using JobBoard.Application.Interfaces;
using JobBoard.Application.Services;
using JobBoard.Infrastructure.Services;

namespace JobBoard.API.Extensions
{
    public static class AIServices
    {
        public static IServiceCollection AddAIServices(this IServiceCollection services)
        {
            services.AddSingleton<IGeminiChatService, GeminiChatService>();
            services.AddScoped<IAIEmbeddingService, AIEmbeddingService>();
            services.AddScoped<IChatHistoryService, ChatHistoryService>();

            return services;
        }
    }
}
