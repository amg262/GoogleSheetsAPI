namespace GoogleSheetsAPI.DTOs;

public class CreateDriveFileRequestDto
{
    public string Name { get; set; }
    public string MimeType { get; set; }
    public string? FolderId { get; set; }
    public string? Content { get; set; }
    public string? ParentId { get; set; }
}