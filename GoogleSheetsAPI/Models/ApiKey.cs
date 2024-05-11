namespace GoogleSheetsAPI.Models;

public class ApiKey
{
    public int Id { get; set; }
    public string? Key { get; set; }
    public bool? IsActive { get; set; } = true;
    public DateTime? CreatedAt { get; set; } = DateTime.Now;
}