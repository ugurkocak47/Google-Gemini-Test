
using Microsoft.Extensions.Options;
using WebAPI.Helpers;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.Configure<GeminiOptions>(builder.Configuration.GetSection("Gemini"));

            builder.Services.AddTransient<GeminiDelegatingHandler>();

            builder.Services.AddHttpClient<GeminiClient>(
                 (serviceProvider, httpClient) =>
                 {
                     var geminiOptions = serviceProvider.GetRequiredService<IOptions<GeminiOptions>>().Value;

                     httpClient.BaseAddress = new Uri(geminiOptions.Url);
                 })
        .AddHttpMessageHandler<GeminiDelegatingHandler>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
