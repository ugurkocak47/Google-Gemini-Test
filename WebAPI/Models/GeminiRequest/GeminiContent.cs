namespace WebAPI.Models.GeminiRequest
{
    public class GeminiContent
    {
        public string Role { get; set; }
        public GeminiPart[] Parts { get; set; }
    }
}
