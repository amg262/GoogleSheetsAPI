namespace GoogleSheetsAPI.DTOs;

public class AnalyticsRequestDto
{
    public string ViewId { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string MetricExpression { get; set; }
    public string DimensionName { get; set; }
}