using GoogleSheetsAPI.Helpers;
using System.Text.Json;
using Xunit;

namespace UnitTests;

/// <summary>
/// Contains unit tests for the <see cref="ApiHelper"/> class.
/// </summary>
public class ApiHelperTests
{
    /// <summary>
    /// Tests conversion of different JsonElement types to their corresponding .NET types.
    /// </summary>
    [Theory]
    [InlineData(JsonValueKind.String, "\"TestString\"", "\"TestString\"")]
    [InlineData(JsonValueKind.Number, "1234", 1234f)]
    [InlineData(JsonValueKind.True, "true", true)]
    [InlineData(JsonValueKind.False, "false", false)]
    [InlineData(JsonValueKind.Null, "null", null)]
    public void Test_GetObjectValue_Conversions(JsonValueKind kind, string json, object? expected)
    {
        // Arrange
        var jsonElement = JsonDocument.Parse(json).RootElement;

        // Act
        var result = ApiHelper.GetObjectValue(jsonElement);

        // Assert
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Tests that GetObjectValue returns the exception message when conversion fails.
    /// </summary>
    [Fact]
    public void Test_GetObjectValue_ConversionFailure()
    {
        // Arrange
        var testClass = new TestClass(1234, "TestString", true, 1234.5f, null);

        // Act
        var result = ApiHelper.GetObjectValue(testClass);

        // Assert
        var s = result as string;
        const string failureText = "Unable to cast object of type 'TestClass' to type 'System.Text.Json.JsonElement";
        Assert.True(s != null && s.Contains(failureText));
    }

    /// <summary>
    /// Tests generating a secure API key.
    /// </summary>
    [Fact]
    public void Test_GenerateSecureApiKey_DefaultSize()
    {
        // Act
        var apiKey = ApiHelper.GenerateSecureApiKey();

        // Assert
        Assert.Equal(88, apiKey.Length); // Base64 length calculation for 64 bytes
    }

    /// <summary>
    /// Tests generating a secure API key with a specific size.
    /// </summary>
    [Fact]
    public void Test_GenerateSecureApiKey_SpecificSize()
    {
        // Arrange
        int size = 32; // 32 bytes

        // Act
        var apiKey = ApiHelper.GenerateSecureApiKey(size);

        // Assert
        Assert.Equal(44, apiKey.Length); // Base64 length calculation for 32 bytes
    }

    internal class TestClass
    {
        public long Long { get; set; }
        public string String { get; set; }
        public bool Bool { get; set; }
        public float Float { get; set; }
        public object? Null { get; set; }

        public TestClass(long l, string s, bool b, float f, object? n)
        {
            Long = l;
            String = s;
            Bool = b;
            Float = f;
            Null = n;
        }
    }
}