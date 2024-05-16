using GoogleSheetsAPI.DTOs;

namespace UnitTests;

/// <summary>
/// Contains unit tests for the <see cref="WriteRequestDto"/> class.
/// </summary>
public class WriteRequestDtoTests
{
    /// <summary>
    /// Tests that the default value of the SpreadsheetId property is correct.
    /// </summary>
    [Fact]
    public void Test_SpreadsheetId_DefaultValue()
    {
        // Arrange
        var writeRequestDto = new WriteRequestDto();

        // Act
        var spreadsheetId = writeRequestDto.SpreadsheetId;

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
        var writeRequestDto = new WriteRequestDto();

        // Act
        var sheetname = writeRequestDto.Sheetname;

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
        var writeRequestDto = new WriteRequestDto();

        // Act
        var range = writeRequestDto.Range;

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
        var writeRequestDto = new WriteRequestDto();

        // Act
        var values = writeRequestDto.Values;

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
        var writeRequestDto = new WriteRequestDto { SpreadsheetId = customSpreadsheetId };

        // Act
        var spreadsheetId = writeRequestDto.SpreadsheetId;

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
        var writeRequestDto = new WriteRequestDto { Sheetname = customSheetname };

        // Act
        var sheetname = writeRequestDto.Sheetname;

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
        var writeRequestDto = new WriteRequestDto { Range = customRange };

        // Act
        var range = writeRequestDto.Range;

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
        var writeRequestDto = new WriteRequestDto { Values = customValues };

        // Act
        var values = writeRequestDto.Values;

        // Assert
        Assert.Equal(customValues, values);
    }
}