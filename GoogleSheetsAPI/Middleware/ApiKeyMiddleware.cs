using GoogleSheetsAPI.Data;

namespace GoogleSheetsAPI.Middleware;

/// <summary>
/// Middleware to enforce API Key authentication on requests.
/// </summary>
public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string ApikeyName = "x-api-key";
    private readonly string _apiKey;
    private readonly ILogger<ApiKeyMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of <see cref="ApiKeyMiddleware"/> with the specified dependencies.
    /// </summary>
    /// <param name="next">The next delegate in the request pipeline.</param>
    /// <param name="config">The configuration where API key settings are stored.</param>
    /// <param name="logger">The logger used to log messages related to API key authentication.</param>
    public ApiKeyMiddleware(RequestDelegate next, IConfiguration config, ILogger<ApiKeyMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        _apiKey = config.GetValue<string>("ApiKey") ?? string.Empty;
    }

    /// <summary>
    /// Invokes the middleware to check the API key provided in the HTTP request headers.
    /// </summary>
    /// <param name="context">The context for the current HTTP request.</param>
    /// <param name="db">The database context used within the middleware, if needed for authentication purposes.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <remarks>
    /// The method checks for the presence of an API key in the request headers. If the key is missing or invalid,
    /// it sets the HTTP status code to 401 (Unauthorized) and terminates the request pipeline.
    /// If the API key is valid, it calls the next delegate/middleware in the pipeline.
    /// </remarks>
    public async Task InvokeAsync(HttpContext context, AppDbContext db)
    {
        if (!context.Request.Headers.TryGetValue(ApikeyName, out var extractedApiKey) ||
            string.IsNullOrWhiteSpace(extractedApiKey))
        {
            _logger.LogWarning("API Key is missing from the headers");
            context.Response.StatusCode = 401;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("API Key is missing");
            return;
        }

        if (extractedApiKey != _apiKey)
        {
            _logger.LogWarning("Invalid API Key provided");
            context.Response.StatusCode = 401;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("Invalid API Key");
            return;
        }

        await _next(context);
    }
}