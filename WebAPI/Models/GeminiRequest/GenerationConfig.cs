namespace WebAPI.Models.GeminiRequest
{
    public class GenerationConfig
    {
        public double Temperature { get; set; }
        public int TopK { get; set; }
        public double TopP { get; set; }
        public int MaxOutputTokens { get; set; }
        public List<object> StopSequences { get; set; }
    }
}
