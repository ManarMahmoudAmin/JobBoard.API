using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Interfaces
{
    public interface IGeminiChatService
    {
        Task<string> AskGeminiAsync(string prompt);
        string GetApiKey();
    }
}
