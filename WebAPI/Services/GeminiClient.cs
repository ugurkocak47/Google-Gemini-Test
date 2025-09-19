using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;
using WebAPI.Models.GeminiResponse;

namespace WebAPI.Services
{
    public class GeminiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GeminiClient> _logger;
        private readonly JsonSerializerSettings _serializerSettings = new()
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };

        public GeminiClient(HttpClient httpClient, ILogger<GeminiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> GenerateContentAsync(string prompt, CancellationToken cancellationToken)
        {
            var requestBody = GeminiRequestFactory.CreateRequest(prompt);
            var jsonContent = JsonConvert.SerializeObject(requestBody, Formatting.None, _serializerSettings);
            
            _logger.LogInformation("Sending request to Gemini API: {JsonContent}", jsonContent);
            
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("", content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Gemini API returned {StatusCode}: {ErrorContent}", response.StatusCode, errorContent);
                throw new HttpRequestException($"Gemini API error ({response.StatusCode}): {errorContent}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Received response from Gemini API: {ResponseBody}", responseBody);

            var geminiResponse = JsonConvert.DeserializeObject<GeminiResponse>(responseBody);

            if (geminiResponse?.Candidates == null || geminiResponse.Candidates.Length == 0)
            {
                throw new InvalidOperationException("No candidates returned from Gemini API");
            }

            var geminiResponseText = geminiResponse.Candidates[0].Content.Parts[0].Text;

            return geminiResponseText;
        }
    }
}
