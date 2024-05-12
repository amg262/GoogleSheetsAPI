using GoogleSheetsAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace GoogleSheetsAPI.Middleware;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string ApikeyName = "x-api-key";

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext db)
    {
        if (!context.Request.Headers.TryGetValue(ApikeyName, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key is missing");
            return;
        }

        var apiKey = await db.ApiKeys.FirstOrDefaultAsync(x => x.Key == extractedApiKey && x.IsActive == true);

        if (apiKey == null)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid API Key");
            return;
        }

        await _next(context);
    }
}