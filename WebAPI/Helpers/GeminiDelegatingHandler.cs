using Microsoft.Extensions.Options;
using WebAPI.Models;

namespace WebAPI.Helpers
{
    public class GeminiDelegatingHandler(IOptions<GeminiOptions> geminiOptions) : DelegatingHandler
    {
        private readonly GeminiOptions _options = geminiOptions.Value;
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("x-goog-api-key", $"{_options.ApiKey}");

            return base.SendAsync(request, cancellationToken);
        }
    }
}
