namespace WebAPI.Models.GeminiResponse
{
    public class GeminiResponse
    {
        public Candidate[] Candidates { get; set; }
        public PromptFeedback PromptFeedback { get; set; }
    }
}
