using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.DTOs.AIEmbeddingDTOs
{
    public class AskQuestionDto
    {

        /* receive the user Id */
        public string UserId { get; set; } = string.Empty;
        /* receive the user Qusetion*/
        public string Question { get; set; }
    }
}
