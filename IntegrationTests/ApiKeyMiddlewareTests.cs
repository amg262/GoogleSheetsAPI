using GoogleSheetsAPI.Data;
using GoogleSheetsAPI.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace IntegrationTests;

/// <summary>
/// Contains integration tests for the ApiKeyMiddleware class.
/// </summary>
[Collection("Shared collection")]

public class ApiKeyMiddlewareTests
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiKeyMiddlewareTests"/> class.
    /// Sets up the configuration by loading the appsettings.json file.
    /// </summary>
    public ApiKeyMiddlewareTests()
    {
        // Build configuration from the appsettings.json file of the referenced project
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        _configuration = builder.Build();
    }

    /// <summary>
    /// Tests the ApiKeyMiddleware to ensure it returns 401 when no API key is provided.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    [Fact]
    public async Task InvokeAsync_NoApiKey_Returns401()
    {
        // Arrange
        var mockNext = new Mock<RequestDelegate>();
        var mockLogger = new Mock<ILogger<ApiKeyMiddleware>>();
        var mockDb = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());

        var middleware = new ApiKeyMiddleware(mockNext.Object, _configuration, mockLogger.Object);
        var context = new DefaultHttpContext();

        // Act
        await middleware.InvokeAsync(context, mockDb.Object);

        // Assert
        Assert.Equal(401, context.Response.StatusCode);
        // Assert.Equal("API Key is missing", await new StreamReader(context.Response.Body).ReadToEndAsync());
    }

    /// <summary>
    /// Tests the ApiKeyMiddleware to ensure it returns 401 when an invalid API key is provided.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    [Fact]
    public async Task InvokeAsync_InvalidApiKey_Returns401()
    {
        // Arrange
        var mockNext = new Mock<RequestDelegate>();
        var mockLogger = new Mock<ILogger<ApiKeyMiddleware>>();
        var mockDb = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());

        var middleware = new ApiKeyMiddleware(mockNext.Object, _configuration, mockLogger.Object);
        var context = new DefaultHttpContext();
        context.Request.Headers["x-api-key"] = "invalid-api-key";

        // Act
        await middleware.InvokeAsync(context, mockDb.Object);

        // Assert
        Assert.Equal(401, context.Response.StatusCode);
        // Assert.Equal("Invalid API Key", await new StreamReader(context.Response.Body).ReadToEndAsync());
    }

    /// <summary>
    /// Tests the ApiKeyMiddleware to ensure it calls the next delegate in the pipeline when a valid API key is provided.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    [Fact]
    public async Task InvokeAsync_ValidApiKey_CallsNextDelegate()
    {
        // Arrange
        var mockNext = new Mock<RequestDelegate>();
        mockNext.Setup(next => next(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);

        var mockLogger = new Mock<ILogger<ApiKeyMiddleware>>();
        var mockDb = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());

        var middleware = new ApiKeyMiddleware(mockNext.Object, _configuration, mockLogger.Object);
        var context = new DefaultHttpContext();
        context.Request.Headers["x-api-key"] = _configuration.GetValue<string>("ApiKey");

        // Act
        await middleware.InvokeAsync(context, mockDb.Object);

        // Assert
        mockNext.Verify(next => next(It.IsAny<HttpContext>()), Times.Once);
    }
}