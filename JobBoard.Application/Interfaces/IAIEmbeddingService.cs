using JobBoard.Application.DTOs.AIEmbeddingDTOs;
using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Interfaces
{
    public interface IAIEmbeddingService
    {
        public Task GenerateEmbeddingsForJobsAsync();
        public Task<List<SemanticSearchResultDto>> SearchJobsByMeaningAsync(string query, int topK);
        //public Task<string> GetJobAnswerFromGeminiAsync(string userQuestion);
        public Task<string> GetJobAnswerFromGeminiAsync(string userId, string userQuestion);

        public Task GenerateEmbeddingForJobAsync(Job job);
        public Task DeleteEmbeddingForJobAsync(int jobId);
    }
}


