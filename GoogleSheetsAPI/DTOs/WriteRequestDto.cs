namespace GoogleSheetsAPI.DTOs;

/// <summary>
/// Data transfer object for writing data to a Google Sheets spreadsheet.
/// </summary>
public record WriteRequestDto
{
    /// <summary>
    /// Gets or sets the ID of the spreadsheet to write to.
    /// Defaults to "1IETU7EI1UKkVGgaCcoz0R0cnX5tdme-6ealsXvtXR1k".
    /// </summary>
    public string? SpreadsheetId { get; set; } = "1IETU7EI1UKkVGgaCcoz0R0cnX5tdme-6ealsXvtXR1k";

    /// <summary>
    /// Gets or sets the name of the sheet to write to.
    /// Defaults to "Sheet1".
    /// </summary>
    public string? Sheetname { get; set; } = "Sheet1";

    /// <summary>
    /// Gets or sets the range within the sheet to write to.
    /// Defaults to "A1:Z1".
    /// </summary>
    public string? Range { get; set; } = "A1:Z1";

    /// <summary>
    /// Gets or sets the list of values to write.
    /// This property should handle mixed data types properly.
    /// </summary>
    public List<object?>? Values { get; set; } // This should handle mixed data types properly
}