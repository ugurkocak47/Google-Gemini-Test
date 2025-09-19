namespace WebAPI.Models.GeminiRequest
{
    public class GeminiRequest
    {
        public GeminiContent[] Contents { get; set; }
        public GenerationConfig GenerationConfig { get; set; }
        public SafetySettings[] SafetySettings { get; set; }
    }
}
