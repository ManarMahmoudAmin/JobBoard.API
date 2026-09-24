namespace JobBoard.Domain.Entities
{
    public class AIEmbedding
    {
        public int Id { get; set; }

        //like "Job" or "Recruiter"
        public string EntityType { get; set; }

        // from Chosen Entity that we convert it to embedding
        public int EntityId { get; set; }      

        //the text converted to embedding
        public string Content { get; set; }

        //response from Gemini
        public float[] EmbeddingVector { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
