using GoogleSheetsAPI.DTOs;

namespace UnitTests;
/// <summary>
/// Contains unit tests for the <see cref="ReadRequestDto"/> class.
/// </summary>
public class ReadRequestDtoTests
{
    /// <summary>
    /// Tests that the default value of the SpreadsheetId property is correct.
    /// </summary>
    [Fact]
    public void Test_SpreadsheetId_DefaultValue()
    {
        // Arrange
        var readRequestDto = new ReadRequestDto();

        // Act
        var spreadsheetId = readRequestDto.SpreadsheetId;

        // Assert
        Assert.Equal("1IETU7EI1UKkVGgaCcoz0R0cnX5tdme-6ealsXvtXR1k", spreadsheetId);
    }
    /// <summary>
    /// Tests that the default value of the Sheetname property is correct.
    /// </summary>
    [Fact]
    public void Test_Sheetname_DefaultValue()
    {
        // Arrange
        var readRequestDto = new ReadRequestDto();

        // Act
        var sheetname = readRequestDto.Sheetname;

        // Assert
        Assert.Equal("Sheet1", sheetname);
    }
    /// <summary>
    /// Tests that the default value of the Range property is correct.
    /// </summary>
    [Fact]
    public void Test_Range_DefaultValue()
    {
        // Arrange
        var readRequestDto = new ReadRequestDto();

        // Act
        var range = readRequestDto.Range;

        // Assert
        Assert.Equal("A1:Z1", range);
    }
    /// <summary>
    /// Tests that the default value of the Values property is null.
    /// </summary>
    [Fact]
    public void Test_Values_DefaultValue()
    {
        // Arrange
        var readRequestDto = new ReadRequestDto();

        // Act
        var values = readRequestDto.Values;

        // Assert
        Assert.Null(values);
    }
    /// <summary>
    /// Tests that the SpreadsheetId property can be set to a custom value.
    /// </summary>
    [Fact]
    public void Test_SpreadsheetId_SetCustomValue()
    {
        // Arrange
        const string customSpreadsheetId = "customSpreadsheetId";
        var readRequestDto = new ReadRequestDto { SpreadsheetId = customSpreadsheetId };

        // Act
        var spreadsheetId = readRequestDto.SpreadsheetId;

        // Assert
        Assert.Equal(customSpreadsheetId, spreadsheetId);
    }
    /// <summary>
    /// Tests that the Sheetname property can be set to a custom value.
    /// </summary>
    [Fact]
    public void Test_Sheetname_SetCustomValue()
    {
        // Arrange
        const string customSheetname = "CustomSheet";
        var readRequestDto = new ReadRequestDto { Sheetname = customSheetname };

        // Act
        var sheetname = readRequestDto.Sheetname;

        // Assert
        Assert.Equal(customSheetname, sheetname);
    }
    /// <summary>
    /// Tests that the Range property can be set to a custom value.
    /// </summary>
    [Fact]
    public void Test_Range_SetCustomValue()
    {
        // Arrange
        const string customRange = "B2:C3";
        var readRequestDto = new ReadRequestDto { Range = customRange };

        // Act
        var range = readRequestDto.Range;

        // Assert
        Assert.Equal(customRange, range);
    }
    /// <summary>
    /// Tests that the Values property can be set to a custom value.
    /// </summary>
    [Fact]
    public void Test_Values_SetCustomValue()
    {
        // Arrange
        var customValues = new List<object?> { 1, "two", 3.0 };
        var readRequestDto = new ReadRequestDto { Values = customValues };

        // Act
        var values = readRequestDto.Values;

        // Assert
        Assert.Equal(customValues, values);
    }
}