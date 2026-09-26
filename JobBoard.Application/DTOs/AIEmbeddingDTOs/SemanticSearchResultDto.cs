using JobBoard.Application.DTOs.JobDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.DTOs.AIEmbeddingDTOs
{
    public class SemanticSearchResultDto
    {
        public JobDto Job { get; set; }
        public double Similarity { get; set; }
    }
}
