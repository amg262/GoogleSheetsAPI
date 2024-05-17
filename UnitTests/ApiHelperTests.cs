using System.Text.Json;
using GoogleSheetsAPI.Helpers;

namespace UnitTests;

/// <summary>
/// Contains unit tests for the <see cref="ApiHelper"/> class, focusing on its ability
/// to convert JSON elements to corresponding .NET types and handle exceptions appropriately.
/// </summary>
public class ApiHelperTests
{
    /// <summary>
    /// Tests that <see cref="ApiHelper.GetObjectValue(object?)"/> returns a specific string ("NULL")
    /// when called with a null input, mimicking handling of null JSON values.
    /// </summary>
    [Fact]
    public void GetObjectValue_ReturnsNull_ForNullInput()
    {
        // Act
        var result = ApiHelper.GetObjectValue(null);

        // Assert
        Assert.Equal("NULL", result);
    }

    /// <summary>
    /// Tests that <see cref="ApiHelper.GetObjectValue(object?)"/> correctly converts various JSON strings
    /// to their corresponding .NET types.
    /// </summary>
    /// <param name="jsonString">The JSON string to parse and convert.</param>
    /// <param name="valueKind">The expected JsonValueKind of the JSON element.</param>
    /// <param name="expected">The expected result of the conversion.</param>
    [Theory]
    [InlineData("123", JsonValueKind.Number, 123.0f)]
    [InlineData("\"hello\"", JsonValueKind.String, "hello")]
    [InlineData("true", JsonValueKind.True, true)]
    [InlineData("false", JsonValueKind.False, false)]
    [InlineData("null", JsonValueKind.Null, null)]
    // [InlineData("undefined", JsonValueKind.Undefined, null)]
    [InlineData("{\"key\":\"value\"}", JsonValueKind.Object, "{\"key\":\"value\"}")]
    [InlineData("[1,2,3]", JsonValueKind.Array, "[1,2,3]")]
    public void GetObjectValue_ConvertsJsonElement_ToCorrectType(string jsonString, JsonValueKind valueKind,
        object expected)
    {
        // Arrange
        var jsonElement = JsonDocument.Parse(jsonString).RootElement;

        // Act
        var result = ApiHelper.GetObjectValue(jsonElement);

        // Assert
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Tests that <see cref="ApiHelper.GetObjectValue(object?)"/> returns an error message
    /// when a non-JsonElement object is passed, indicating improper input type.
    /// </summary>
    [Fact]
    public void GetObjectValue_ThrowsArgumentException_ForNonJsonElement()
    {
        // Arrange
        var testClass = new TestClass();

        // Act
        var result = ApiHelper.GetObjectValue(testClass);

        // Assert
        Assert.StartsWith("Error:", result as string); // Check if the return starts with 'Error:'
    }

    /// <summary>
    /// Tests the conversion of different JsonElement types to their corresponding .NET types,
    /// ensuring that <see cref="ApiHelper.GetObjectValue(object?)"/> handles each type accurately.
    /// </summary>
    [Theory]
    [InlineData(JsonValueKind.String, "\"TestString\"", "TestString")]
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
    /// Tests that <see cref="ApiHelper.GetObjectValue(object?)"/> returns the appropriate exception message
    /// when conversion fails due to improper input, such as a non-JsonElement object.
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
        const string failureText = "Error: Expected a JsonElement object (Parameter 'obj')";
        Assert.True(s != null && s.Contains(failureText));
    }

    /// <summary>
    /// A test class used for simulating conversion failures, helping to test the robustness of the 
    /// <see cref="ApiHelper.GetObjectValue(object?)"/> method in handling improper input types.
    /// </summary>
    private class TestClass
    {
        public int IntValue { get; set; }
        public string StringValue { get; set; }
        public bool BoolValue { get; set; }
        public float FloatValue { get; set; }
        public object? NullValue { get; set; }

        public TestClass(int intValue, string stringValue, bool boolValue, float floatValue, object? nullValue)
        {
            IntValue = intValue;
            StringValue = stringValue;
            BoolValue = boolValue;
            FloatValue = floatValue;
            NullValue = nullValue;
        }

        public TestClass()
        {
            IntValue = 1234;
            StringValue = "stringValue";
            BoolValue = true;
            FloatValue = 1234.5f;
            NullValue = null;
        }
    }
}