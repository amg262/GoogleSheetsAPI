using System.Text.Json.Serialization;

namespace GoogleSheetsAPI.DTOs;

/// <summary>
/// Represents a request for updating text within a specified range in a Google Document.
/// </summary>
public record UpdateTextRequestDto
{
    /// <summary>
    /// Gets or sets the starting index of the text range to update.
    /// Nullable to allow flexibility in specifying the range.
    /// </summary>
    [JsonPropertyName("startIndex")]
    public int? StartIndex { get; set; }

    /// <summary>
    /// Gets or sets the ending index of the text range to update.
    /// The ending index must be greater than the starting index.
    /// Nullable to allow flexibility in specifying the range.
    /// </summary>
    [JsonPropertyName("endIndex")]
    public int? EndIndex { get; set; }

    /// <summary>
    /// Gets or sets the text to insert at the specified range.
    /// The existing text within the range will be replaced with this text.
    /// Nullable to provide the option of clearing text without insertion.
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; set; }
}