using JobBoard.Application.DTOs.AIEmbeddingDTOs;

namespace JobBoard.Application.Interfaces
{
    public interface IChatHistoryService
    {
        Task AddMessageAsync(string userId, string message);
        Task<List<string>> GetMessagesAsync(string userId);
        Task ClearHistoryAsync(string userId);

        /*---------------------Add Chat History---------------------*/
        Task AddAsync(string userId, ChatMessageDto message);
        Task<IReadOnlyList<ChatMessageDto>> GetAsync(string userId, int takeLast = 20);
        Task ClearAsync(string userId);
    }

}
