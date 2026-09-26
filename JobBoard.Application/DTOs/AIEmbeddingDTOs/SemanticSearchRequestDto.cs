using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.DTOs.AIEmbeddingDTOs
{
    public class SemanticSearchRequestDto
    {
        public string Query { get; set; }
        public int TopK { get; set; } = 5;
    }
}
