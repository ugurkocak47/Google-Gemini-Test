using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly GeminiClient _geminiClient;
        private readonly ILogger<HomeController> _logger;

        public HomeController(GeminiClient geminiClient, ILogger<HomeController> logger)
        {
            _geminiClient = geminiClient;
            _logger = logger;
        }

        [HttpPost("test")]
        public async Task<IActionResult> TestGemini([FromBody] string prompt, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(prompt))
                {
                    return BadRequest("Prompt cannot be empty");
                }

                var response = await _geminiClient.GenerateContentAsync(prompt, cancellationToken);
                
                return Ok(new { 
                    success = true, 
                    response = response,
                    prompt = prompt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling Gemini API with prompt: {Prompt}", prompt);
                return StatusCode(500, new { 
                    success = false, 
                    error = ex.Message 
                });
            }
        }

        [HttpGet("quick-test")]
        public async Task<IActionResult> QuickTest(CancellationToken cancellationToken = default)
        {
            try
            {
                var testPrompt = "Hello, how are you today?";
                var response = await _geminiClient.GenerateContentAsync(testPrompt, cancellationToken);
                
                return Ok(new { 
                    success = true, 
                    response = response,
                    prompt = testPrompt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in quick test");
                return StatusCode(500, new { 
                    success = false, 
                    error = ex.Message 
                });
            }
        }
    }
}
