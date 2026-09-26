using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.DTOs.AIEmbeddingDTOs
{
    public class ChatMessageDto
    {
        public string Role { get; set; } = ""; // "user" / "Assistant"
        public string Content { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
