using Google.Apis.Docs.v1;
using Google.Apis.Sheets.v4;

namespace GoogleSheetsAPI.Models;

public class GoogleServices
{
    public DocsService DocsService { get; }
    public SheetsService SheetsService { get; }

    public GoogleServices(DocsService docsService, SheetsService sheetsService)
    {
        DocsService = docsService;
        SheetsService = sheetsService;
    }
}