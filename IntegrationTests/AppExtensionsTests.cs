using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests;

/// <summary>
/// Represents the collection of integration tests for the API.
/// These tests ensure that the API endpoints perform as expected when the application is running.
/// </summary>
[Collection("Shared collection")]
public class ApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiIntegrationTests"/> class.
    /// Uses the provided <see cref="WebApplicationFactory{TEntryPoint}"/> to create instances of the application for testing.
    /// </summary>
    /// <param name="factory">The factory used to create instances of the application under test.</param>
    public ApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// Verifies that the Swagger endpoint returns a successful response and that the content type is correct.
    /// This test checks the availability and configuration of the Swagger documentation generated for the API.
    /// </summary>
    [Fact]
    public async Task SwaggerEndpoint_ReturnsSuccess_And_CorrectContentType()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/swagger/v1/swagger.json");

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }
}