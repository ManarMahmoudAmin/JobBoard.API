using GenerativeAI;
using JobBoard.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Infrastructure.Services
{
    public class GeminiChatService : IGeminiChatService
    {
        private readonly string _apiKey;
        private readonly GenerativeModel _model;

        public GeminiChatService(IConfiguration configuration)
        {
            _apiKey = configuration["Gemini:ApiKey"];
            _model = new GenerativeModel(model: "gemini-1.5-flash", apiKey: _apiKey);
        }
        public async Task<string> AskGeminiAsync(string prompt)
        {
            var result = await _model.GenerateContentAsync(prompt);
            return result.Text;
        }

        public string GetApiKey() => _apiKey;
    }
}