using GoogleSheetsAPI.DTOs;
using Xunit;

namespace UnitTests;

/// <summary>
/// Contains unit tests for the <see cref="AppendTextRequestDto"/> class.
/// </summary>
public class AppendTextRequestDtoTests
{
    /// <summary>
    /// Tests that the default value of the Text property is null.
    /// </summary>
    [Fact]
    public void Test_Text_DefaultValue()
    {
        // Arrange
        var appendTextRequest = new AppendTextRequestDto();

        // Act
        var text = appendTextRequest.Text;

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
        const string customText = "Example text";
        var appendTextRequest = new AppendTextRequestDto { Text = customText };

        // Act
        var text = appendTextRequest.Text;

        // Assert
        Assert.Equal(customText, text);
    }
}