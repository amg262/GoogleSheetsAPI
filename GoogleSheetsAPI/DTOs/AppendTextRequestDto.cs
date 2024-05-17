using System.Text.Json.Serialization;

namespace GoogleSheetsAPI.DTOs;

public record AppendTextRequestDto
{
    [JsonPropertyName("text")] public string? Text { get; set; }
}