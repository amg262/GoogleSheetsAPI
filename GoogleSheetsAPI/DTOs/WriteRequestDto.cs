namespace GoogleSheetsAPI.DTOs;

public class WriteRequestDto
{
    public string SpreadsheetId { get; set; }
    public string Sheetname { get; set; }
    public string Range { get; set; }
    public List<List<object>> Values { get; set; }
}