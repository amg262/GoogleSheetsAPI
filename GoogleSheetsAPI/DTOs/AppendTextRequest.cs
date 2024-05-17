using System.Text.Json.Serialization;

namespace GoogleSheetsAPI.DTOs;

public record AppendTextRequest
{
    [JsonPropertyName("text")] public string? Text { get; set; }
}