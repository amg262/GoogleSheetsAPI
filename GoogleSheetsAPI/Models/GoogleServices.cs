using Google.Apis.AnalyticsReporting.v4;
using Google.Apis.Docs.v1;
using Google.Apis.Drive.v3;
using Google.Apis.Sheets.v4;

namespace GoogleSheetsAPI.Models;

/// <summary>
/// Provides centralized access to various Google services used within the application.
/// This class encapsulates the service clients for Google Sheets, Google Docs, Google Drive,
/// and Google Analytics Reporting, allowing for streamlined management and usage throughout the application.
/// </summary>
public class GoogleServices
{
    /// <summary>
    /// Gets the Google Sheets service client used to interact with the Google Sheets API.
    /// </summary>
    public SheetsService SheetsService { get; }

    /// <summary>
    /// Gets the Google Docs service client used to interact with the Google Docs API.
    /// </summary>
    public DocsService DocsService { get; }

    /// <summary>
    /// Gets or sets the Google Drive service client used to interact with the Google Drive API.
    /// This client facilitates operations like file uploads, downloads, and management of Drive resources.
    /// </summary>
    public DriveService DriveService { get; set; }

    /// <summary>
    /// Gets or sets the Google Analytics Reporting service client used to fetch analytical data.
    /// This service is used to create reports and fetch analytics based on user interactions with web properties.
    /// </summary>
    public AnalyticsReportingService ReportingService { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GoogleServices"/> class,
    /// creating service clients for Google Sheets, Docs, Drive, and Analytics Reporting.
    /// </summary>
    /// <param name="sheetsService">The initialized service client for Google Sheets.</param>
    /// <param name="docsService">The initialized service client for Google Docs.</param>
    /// <param name="driveService">The initialized service client for Google Drive.</param>
    /// <param name="reportingService">The initialized service client for Google Analytics Reporting.</param>
    public GoogleServices(SheetsService sheetsService, DocsService docsService, DriveService driveService,
        AnalyticsReportingService reportingService)
    {
        SheetsService = sheetsService;
        DocsService = docsService;
        DriveService = driveService;
        ReportingService = reportingService;
    }
}