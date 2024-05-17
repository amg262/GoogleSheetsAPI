using System.Text.Json.Serialization;

namespace GoogleSheetsAPI.DTOs;

/// <summary>
/// Data transfer object for reading data from a Google Sheets spreadsheet.
/// </summary>
public record ReadRequestDto
{
    /// <summary>
    /// Gets or sets the ID of the spreadsheet to read from.
    /// Defaults to "1IETU7EI1UKkVGgaCcoz0R0cnX5tdme-6ealsXvtXR1k".
    /// </summary>
    [JsonPropertyName("spreadsheetId")]
    public string? SpreadsheetId { get; set; } = "1IETU7EI1UKkVGgaCcoz0R0cnX5tdme-6ealsXvtXR1k";

    /// <summary>
    /// Gets or sets the name of the sheet to read from.
    /// Defaults to "Sheet1".
    /// </summary>
    [JsonPropertyName("sheetname")]
    public string? Sheetname { get; set; } = "Sheet1";

    /// <summary>
    /// Gets or sets the range within the sheet to read from.
    /// Defaults to "A1:Z1".
    /// </summary>
    [JsonPropertyName("range")]
    public string? Range { get; set; } = "A1:Z1";

    /// <summary>
    /// Gets or sets the list of values to read.
    /// This property should handle mixed data types properly.
    /// </summary>
    [JsonPropertyName("values")]
    public List<object?>? Values { get; set; } // This should handle mixed data types properly
}