using GoogleSheetsAPI.DTOs;
using Xunit;

namespace UnitTests;

/// <summary>
/// Contains unit tests for the <see cref="DeleteTextRequestDto"/> class.
/// </summary>
public class DeleteTextRequestDtoTests
{
    /// <summary>
    /// Tests that the default value of the StartIndex property is null.
    /// </summary>
    [Fact]
    public void Test_StartIndex_DefaultValue()
    {
        // Arrange
        var deleteTextRequest = new DeleteTextRequestDto();

        // Act
        var startIndex = deleteTextRequest.StartIndex;

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
        const int customStartIndex = 10;
        var deleteTextRequest = new DeleteTextRequestDto { StartIndex = customStartIndex };

        // Act
        var startIndex = deleteTextRequest.StartIndex;

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
        var deleteTextRequest = new DeleteTextRequestDto();

        // Act
        var endIndex = deleteTextRequest.EndIndex;

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
        const int customEndIndex = 20;
        var deleteTextRequest = new DeleteTextRequestDto { EndIndex = customEndIndex };

        // Act
        var endIndex = deleteTextRequest.EndIndex;

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
        var deleteTextRequest = new DeleteTextRequestDto();

        // Act
        var text = deleteTextRequest.Text;

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
        const string customText = "New text content";
        var deleteTextRequest = new DeleteTextRequestDto { Text = customText };

        // Act
        var text = deleteTextRequest.Text;

        // Assert
        Assert.Equal(customText, text);
    }
}