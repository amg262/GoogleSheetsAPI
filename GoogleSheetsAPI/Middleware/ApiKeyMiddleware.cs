using GoogleSheetsAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace GoogleSheetsAPI.Middleware;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _config;
    private const string ApikeyName = "x-api-key";
    private readonly string _apiKey;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration config)
    {
        _next = next;
        _config = config;
        _apiKey = _config.GetValue<string>("ApiKey");
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext db)
    {
        if (!context.Request.Headers.TryGetValue(ApikeyName, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key is missing");
            return;
        }

        if (string.IsNullOrWhiteSpace(extractedApiKey)) 
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key is missing");
            return;
        }
        
        if (extractedApiKey != _apiKey)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid API Key");
            return;
        }
        
        if (extractedApiKey == _apiKey)
        {
            await _next(context);
        }
        

        // await _next(context);
    }
}