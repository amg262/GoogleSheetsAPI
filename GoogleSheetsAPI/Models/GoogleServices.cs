using Google.Apis.Docs.v1;
using Google.Apis.Drive.v3;
using Google.Apis.Sheets.v4;

namespace GoogleSheetsAPI.Models;

public class GoogleServices
{
    public SheetsService SheetsService { get; }
    public DocsService DocsService { get; }
    public DriveService DriveService { get; set; }

    public GoogleServices(SheetsService sheetsService, DocsService docsService, DriveService driveService)
    {
        SheetsService = sheetsService;
        DocsService = docsService;
        DriveService = driveService;
    }
}