using GoogleSheetsAPI.DTOs;
using Xunit;

namespace UnitTests;

/// <summary>
/// Contains unit tests for the <see cref="UpdateTextRequestDto"/> class.
/// </summary>
public class UpdateTextRequestDtoTests
{
    /// <summary>
    /// Tests that the default value of the StartIndex property is null.
    /// </summary>
    [Fact]
    public void Test_StartIndex_DefaultValue()
    {
        // Arrange
        var updateTextRequest = new UpdateTextRequestDto();

        // Act
        var startIndex = updateTextRequest.StartIndex;

        // Assert
        Assert.Null(startIndex);
    }

    /// <summary>
    /// Tests that the StartIndex property can be set to a custom value.
    /// </summary>
    [Fact]
    public void Test_StartIndex_SetCustomValue()
    {
        // Arrange
        const int customStartIndex = 5;
        var updateTextRequest = new UpdateTextRequestDto { StartIndex = customStartIndex };

        // Act
        var startIndex = updateTextRequest.StartIndex;

        // Assert
        Assert.Equal(customStartIndex, startIndex);
    }

    /// <summary>
    /// Tests that the default value of the EndIndex property is null.
    /// </summary>
    [Fact]
    public void Test_EndIndex_DefaultValue()
    {
        // Arrange
        var updateTextRequest = new UpdateTextRequestDto();

        // Act
        var endIndex = updateTextRequest.EndIndex;

        // Assert
        Assert.Null(endIndex);
    }

    /// <summary>
    /// Tests that the EndIndex property can be set to a custom value.
    /// </summary>
    [Fact]
    public void Test_EndIndex_SetCustomValue()
    {
        // Arrange
        const int customEndIndex = 10;
        var updateTextRequest = new UpdateTextRequestDto { EndIndex = customEndIndex };

        // Act
        var endIndex = updateTextRequest.EndIndex;

        // Assert
        Assert.Equal(customEndIndex, endIndex);
    }

    /// <summary>
    /// Tests that the default value of the Text property is null.
    /// </summary>
    [Fact]
    public void Test_Text_DefaultValue()
    {
        // Arrange
        var updateTextRequest = new UpdateTextRequestDto();

        // Act
        var text = updateTextRequest.Text;

        // Assert
        Assert.Null(text);
    }

    /// <summary>
    /// Tests that the Text property can be set to a custom value.
    /// </summary>
    [Fact]
    public void Test_Text_SetCustomValue()
    {
        // Arrange
        const string customText = "Here is some text to replace the old text.";
        var updateTextRequest = new UpdateTextRequestDto { Text = customText };

        // Act
        var text = updateTextRequest.Text;

        // Assert
        Assert.Equal(customText, text);
    }
}