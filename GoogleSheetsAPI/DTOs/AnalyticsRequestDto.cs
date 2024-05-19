using System.Text.Json.Serialization;

namespace GoogleSheetsAPI.DTOs;

/// <summary>
/// Represents a data transfer object for making requests to Google Analytics.
/// This DTO captures the necessary parameters to fetch analytics data such as metrics and dimensions over a specified date range.
/// </summary>
public class AnalyticsRequestDto
{
    /// <summary>
    /// Gets or sets the view ID of the Google Analytics view from which data is requested.
    /// The view ID is specific to a particular view within a Google Analytics account.
    /// </summary>
    public string ViewId { get; set; } = "44890";

    /// <summary>
    /// Gets or sets the start date for the data request in YYYY-MM-DD format.
    /// This date specifies the beginning of the date range for the analytics data.
    /// </summary>
    public string StartDate { get; set; } = DateTime.Today.ToString();

    /// <summary>
    /// Gets or sets the end date for the data request in YYYY-MM-DD format.
    /// This date specifies the end of the date range for the analytics data.
    /// </summary>
    public string EndDate { get; set; } = DateTime.Today.AddYears(-1).ToString();

    /// <summary>
    /// Gets or sets the metric expression for the data request.
    /// Metrics are quantifiable measures used to gauge the performance of the business process.
    /// Example of a metric might be 'ga:sessions' or 'ga:pageviews'.
    /// </summary>
    [JsonPropertyName("metrics")]
    public string MetricExpression { get; set; }

    /// <summary>
    /// Gets or sets the name of the dimension for the data request.
    /// Dimensions are attributes of your data. For example, the dimension 'ga:city'
    /// indicates the city, from which sessions originate.
    /// </summary>
    [JsonPropertyName("dimensions")]
    public string DimensionName { get; set; }
}