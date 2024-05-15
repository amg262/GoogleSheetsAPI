namespace GoogleSheetsAPI.DTOs;

public class WriteRequestDto
{
    public string? SpreadsheetId { get; set; } = "1IETU7EI1UKkVGgaCcoz0R0cnX5tdme-6ealsXvtXR1k";
    public string? Sheetname { get; set; } = "Sheet1";
    public string? Range { get; set; } = "A1:Z1";
    public IList<string?> Values { get; set; }  // This should handle mixed data types properly
}