namespace GoogleSheetsAPI.Models;

public class Spreadsheet
{
    public string? SpreadsheetId { get; set; } = "1IETU7EI1UKkVGgaCcoz0R0cnX5tdme-6ealsXvtXR1k";
    public string? Sheetname { get; set; } = "Sheet1";
    public string? Range { get; set; } = "A1:Z1";
    public List<List<object?>> Values { get; set; } = [["1", "2", "3", "4", 14]];
}