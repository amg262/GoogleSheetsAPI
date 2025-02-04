using Google;
using Google.Apis.AnalyticsReporting.v4;
using Google.Apis.AnalyticsReporting.v4.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Docs.v1;
using Google.Apis.Docs.v1.Data;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using GoogleSheetsAPI.Data;
using GoogleSheetsAPI.DTOs;
using GoogleSheetsAPI.Helpers;
using GoogleSheetsAPI.Middleware;
using GoogleSheetsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dimension = Google.Apis.AnalyticsReporting.v4.Data.Dimension;

// using Dimension = Google.Apis.Docs.v1.Data.Dimension;
using Range = Google.Apis.Docs.v1.Data.Range;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddSingleton(s =>
{
    const string json = "j.json";
    var env = s.GetRequiredService<IWebHostEnvironment>();
    var path = Path.Combine(env.WebRootPath, json);
    var sheetCredential = GoogleCredential.FromFile(path).CreateScoped(SheetsService.Scope.Spreadsheets);
    var docCredential = GoogleCredential.FromFile(path).CreateScoped(DocsService.Scope.Documents);
    var driveCredential = GoogleCredential.FromFile(path).CreateScoped(DriveService.Scope.Drive);
    var analyticsCredential =
        GoogleCredential.FromFile(path).CreateScoped(AnalyticsReportingService.Scope.Analytics).UnderlyingCredential as
            ServiceAccountCredential;

    var sheetsService = new SheetsService(new BaseClientService.Initializer()
    {
        HttpClientInitializer = sheetCredential,
        ApplicationName = "Google Sheets Minimal API",
    });

    var docsService = new DocsService(new BaseClientService.Initializer()
    {
        HttpClientInitializer = docCredential,
        ApplicationName = "Google Docs Minimal API",
    });

    var driveService = new DriveService(new BaseClientService.Initializer()
    {
        HttpClientInitializer = driveCredential,
        ApplicationName = "Google Drive Minimal API",
    });

    var analyticsService = new AnalyticsReportingService(new BaseClientService.Initializer()
    {
        HttpClientInitializer = analyticsCredential,
        ApplicationName = "Google Analytics API",
        HttpClientFactory = new Google.Apis.Http.HttpClientFactory()
    });

    // Todo: Create service for Google Analytics

    return new GoogleServices(sheetsService, docsService, driveService, analyticsService);
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();
builder.Services.AddSwaggerMetadata();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Local",
        corsPolicyBuilder => { corsPolicyBuilder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin(); });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Google Sheets API v1");
        options.DocumentTitle = "Google Sheets API Documentation";
        options.DisplayRequestDuration();
        // options.EnableFilter();
    });
}

app.UseHttpsRedirection();
// app.UseMiddleware<ApiKeyMiddleware>();
app.UseCors("Local");
app.MapHealthCheckEndpoints();

// Define endpoints here.
app.MapPost("api/sheets/write", async (GoogleServices googleServices, [FromBody] WriteRequestDto dto) =>
    {
        var fullRange = $"{dto.Sheetname ?? "Sheet1"}!{dto.Range ?? "A1"}";
        var objList = dto.Values.Select(ApiHelper.GetObjectValue).ToList();

        var valueRange = new Google.Apis.Sheets.v4.Data.ValueRange() { Values = new List<IList<object>> { objList } };
        var request = googleServices.SheetsService.Spreadsheets.Values.Update(valueRange, dto.SpreadsheetId, fullRange);

        request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
        var response = await request.ExecuteAsync();
        return Results.Ok(response.UpdatedRange);
    }).WithName("WriteData")
    .WithTags("Google Sheets")
    .AddOpenApiDefaults("Write Data to Google Sheets based on request body.", "WriteRequestDto");


app.MapGet("api/sheets/read", async (GoogleServices googleServices, [FromBody] ReadRequestDto dto) =>
    {
        var fullRange = $"{dto.Sheetname ?? "Sheet1"}!{dto.Range ?? "A1:Z15"}";
        var request = googleServices.SheetsService.Spreadsheets.Values.Get(dto.SpreadsheetId, fullRange);

        var response = await request.ExecuteAsync();
        return Results.Ok(response.Values);
    }).WithName("ReadData")
    .WithTags("Google Sheets")
    .AddOpenApiDefaults("Read Data from Google Sheets based on request body.", "ReadRequestDto");

app.MapGet("api/sheets/read/{speadsheetId}",
        async (string speadsheetId, GoogleServices googleServices, [FromBody] ReadRequestDto dto) =>
        {
            var fullRange = $"{dto.Sheetname ?? "Sheet1"}!{dto.Range ?? "A1:Z15"}";
            var request = googleServices.SheetsService.Spreadsheets.Values.Get(speadsheetId, fullRange);
            // var request = googleServices.SheetsService.Spreadsheets.Values.Get(dto.SpreadsheetId, fullRange);

            var response = await request.ExecuteAsync();
            return Results.Ok(response.Values);
        }).WithName("ReadSpreadsheetData")
    .WithTags("Google Sheets")
    .AddOpenApiDefaults("Read Data from Google Sheets based on request body.", "ReadRequestDto");


app.MapPut("api/sheets/update", async (GoogleServices googleServices, [FromBody] WriteRequestDto dto) =>
    {
        var fullRange = $"{dto.Sheetname ?? "Sheet1"}!{dto.Range ?? "A1:Z1"}";
        var objList = dto.Values.Select(ApiHelper.GetObjectValue).ToList();

        var valueRange = new Google.Apis.Sheets.v4.Data.ValueRange() { Values = new List<IList<object>> { objList } };
        var request = googleServices.SheetsService.Spreadsheets.Values.Update(valueRange, dto.SpreadsheetId, fullRange);

        request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
        var response = await request.ExecuteAsync();
        return Results.Ok(response.UpdatedRange);
    }).WithName("UpdateData")
    .WithTags("Google Sheets")
    .AddOpenApiDefaults("Update Data in Google Sheets based on request body.", "WriteRequestDto");


app.MapDelete("api/sheets/delete", async (GoogleServices googleServices, [FromBody] WriteRequestDto dto) =>
    {
        var fullRange = $"{dto.Sheetname ?? "Sheet1"}!{dto.Range ?? "A1:Z1"}";
        var request = googleServices.SheetsService.Spreadsheets.Values.Clear(null, dto.SpreadsheetId, fullRange);

        var response = await request.ExecuteAsync();
        return Results.Ok(response.ClearedRange);
    }).WithName("DeleteData")
    .WithTags("Google Sheets")
    .AddOpenApiDefaults("Delete Data in Google Sheets based on request body.", "WriteRequestDto");


app.MapPatch("api/sheets/patch", async (GoogleServices googleServices, [FromBody] WriteRequestDto dto) =>
    {
        var fullRange = $"{dto.Sheetname ?? "Sheet1"}!{dto.Range ?? "A1:Z1"}";
        var objList = dto.Values.Select(ApiHelper.GetObjectValue).ToList();

        var valueRange = new Google.Apis.Sheets.v4.Data.ValueRange() { Values = new List<IList<object>> { objList } };
        var request = googleServices.SheetsService.Spreadsheets.Values.Append(valueRange, dto.SpreadsheetId, fullRange);

        request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
        var response = await request.ExecuteAsync();
        return Results.Ok(response.Updates.UpdatedRange);
    }).WithName("PatchData")
    .WithTags("Google Sheets")
    .AddOpenApiDefaults("Patch Data in Google Sheets based on request body.", "WriteRequestDto");


app.MapPost("api/docs/create", async (GoogleServices googleServices) =>
    {
        var doc = new Document { Title = "New Document" };
        var request = googleServices.DocsService.Documents.Create(doc);
        var result = await request.ExecuteAsync();
        return Results.Ok(new { DocumentId = result.DocumentId });
    }).WithName("CreateDocument")
    .WithTags("Google Docs")
    .AddOpenApiDefaults("Create a new Google Doc.", "none");


app.MapGet("api/docs/read/{documentId}", async (GoogleServices googleServices, string? documentId) =>
    {
        var id = "1JDtuXj5OuCg3kaxayfed0b2IuN4b2UYBu459_lvUpko";
        // var request = googleServices.DocsService.Documents.Get(id);
        var request = googleServices.DocsService.Documents.Get(documentId);
        var document = await request.ExecuteAsync();
        return Results.Ok(document.Body.Content);
    }).WithName("ReadDocument")
    .WithTags("Google Docs")
    .AddOpenApiDefaults("Read content from a Google Doc.", "none");


app.MapPut("api/docs/update/{documentId}",
        async (GoogleServices googleServices, string documentId, [FromBody] List<Request> requests) =>
        {
            var batchUpdateDocumentRequest = new BatchUpdateDocumentRequest { Requests = requests };
            var request = googleServices.DocsService.Documents.BatchUpdate(batchUpdateDocumentRequest, documentId);
            var response = await request.ExecuteAsync();
            return Results.Ok(response);
        }).WithName("UpdateDocument")
    .WithTags("Google Docs")
    .AddOpenApiDefaults("Update a Google Doc based on request body.", "List<Request>");


app.MapPut("api/docs/replace/{documentId}", async (GoogleServices googleServices, string documentId) =>
    {
        // Here we clear the document by replacing the content with an empty string
        var requests = new List<Request>
        {
            new()
            {
                ReplaceAllText = new ReplaceAllTextRequest
                {
                    ContainsText = new SubstringMatchCriteria
                    {
                        Text = "the",
                        MatchCase = false
                    },
                    ReplaceText = "THEE"
                }
            }
        };
        var batchRequest = new BatchUpdateDocumentRequest { Requests = requests };
        var request = googleServices.DocsService.Documents.BatchUpdate(batchRequest, documentId);
        var result = await request.ExecuteAsync();

        return result.Replies == null
            ? Results.BadRequest("Failed to clear document content.")
            : Results.Ok(new { Result = "Document content cleared." });
    }).WithName("DeleteDocumentContent")
    .WithTags("Google Docs")
    .AddOpenApiDefaults("Delete (clear) content in a Google Doc.", "none");


app.MapPost("api/docs/append/{documentId}",
        async (GoogleServices googleServices, string documentId, [FromBody] AppendTextRequestDto appendTextRequest) =>
        {
            if (string.IsNullOrEmpty(appendTextRequest.Text))
            {
                return Results.BadRequest("Text to append must not be empty.");
            }

            // First, get the document to find the correct index for appending text
            var getDocRequest = googleServices.DocsService.Documents.Get(documentId);
            var doc = await getDocRequest.ExecuteAsync();
            var contentLength = doc.Body.Content.LastOrDefault()?.EndIndex ?? 1; // Safe default if document is empty


            // Create a request to append text to the end of the document.
            var requests = new List<Request>
            {
                new()
                {
                    InsertText = new InsertTextRequest
                    {
                        Location = new Location
                        {
                            // Assuming you want to append text at the end of the document.
                            // To insert text elsewhere, you would need to specify a different index.
                            Index = contentLength - 1 // Append at the end of the document
                        },
                        Text = appendTextRequest.Text
                    }
                }
            };

            var batchUpdateRequest = new BatchUpdateDocumentRequest { Requests = requests };
            var request = googleServices.DocsService.Documents.BatchUpdate(batchUpdateRequest, documentId);
            try
            {
                var response = await request.ExecuteAsync();
                return Results.Ok(new
                    { Message = "Text appended successfully.", DocumentId = documentId, Changes = response.Replies });
            }
            catch (Exception ex)
            {
                return Results.Problem("Failed to append text: " + ex.Message);
            }
        }).WithName("AppendTextToDocument")
    .WithTags("Google Docs")
    .AddOpenApiDefaults("Append text to a Google Doc based on document ID and request body.", "AppendTextRequestDto");


app.MapPatch("api/docs/update/{documentId}",
        async (GoogleServices googleServices, string documentId, [FromBody] UpdateTextRequestDto updateRequest) =>
        {
            if (string.IsNullOrEmpty(updateRequest.Text))
            {
                return Results.BadRequest("Text to update must not be empty.");
            }

            // Create the requests to replace text in the specified range.
            var requests = new List<Request>
            {
                new()
                {
                    DeleteContentRange = new DeleteContentRangeRequest
                    {
                        Range = new Range()
                        {
                            StartIndex = updateRequest.StartIndex,
                            EndIndex = updateRequest.EndIndex
                        }
                    }
                },
                new()
                {
                    InsertText = new InsertTextRequest
                    {
                        Location = new Location
                        {
                            Index = updateRequest.StartIndex
                        },
                        Text = updateRequest.Text
                    }
                }
            };

            var batchUpdateRequest = new BatchUpdateDocumentRequest { Requests = requests };
            var request = googleServices.DocsService.Documents.BatchUpdate(batchUpdateRequest, documentId);
            try
            {
                var response = await request.ExecuteAsync();
                return Results.Ok(new
                {
                    Message = "Text updated successfully.",
                    DocumentId = documentId,
                    Changes = response.Replies
                });
            }
            catch (Exception ex)
            {
                return Results.Problem("Failed to update text: " + ex.Message);
            }
        }).WithName("UpdateTextInDocument")
    .WithTags("Google Docs")
    .AddOpenApiDefaults("Update text in a Google Doc based on document ID and request body.", "UpdateTextRequestDto");


app.MapDelete("api/docs/delete/{documentId}",
        async (GoogleServices googleServices, string documentId, [FromBody] DeleteTextRequestDto deleteRequest) =>
        {
            // Create the requests to delete text in the specified range.
            var requests = new List<Request>
            {
                new()
                {
                    DeleteContentRange = new DeleteContentRangeRequest
                    {
                        Range = new Range()
                        {
                            StartIndex = deleteRequest.StartIndex,
                            EndIndex = deleteRequest.EndIndex
                        }
                    }
                }
            };

            var batchUpdateRequest = new BatchUpdateDocumentRequest { Requests = requests };
            var request = googleServices.DocsService.Documents.BatchUpdate(batchUpdateRequest, documentId);
            try
            {
                var response = await request.ExecuteAsync();
                return Results.Ok(new
                {
                    Message = "Text deleted successfully.",
                    DocumentId = documentId,
                    Changes = response.Replies
                });
            }
            catch (Exception ex)
            {
                return Results.Problem("Failed to delete text: " + ex.Message);
            }
        }).WithName("DeleteTextInDocument")
    .WithTags("Google Docs")
    .AddOpenApiDefaults("Delete text in a Google Doc based on document ID and request body.", "DeleteTextRequestDto");


app.MapPost("api/drive/create", async (GoogleServices googleServices, [FromBody] CreateDriveFileRequestDto dto) =>
    {
        var file = new Google.Apis.Drive.v3.Data.File
        {
            Name = dto.Name,
            MimeType = dto.MimeType,
            Parents = new List<string> { dto.ParentId }
        };

        var request = googleServices.DriveService.Files.Create(file);
        var result = await request.ExecuteAsync();
        return Results.Ok(new { FileId = result.Id });
    }).WithName("CreateDriveFile")
    .WithTags("Google Drive")
    .AddOpenApiDefaults("Fetch analytics data based on metrics, dimensions, and date range.", "AnalyticsRequestDto");
// Console.WriteLine(DoIt());


// this isnt gonna work
app.MapPost("api/analytics/report", async (GoogleServices googleServices, [FromBody] AnalyticsRequestDto requestDto) =>
    {
        var dateRange = new DateRange { StartDate = requestDto.StartDate, EndDate = requestDto.EndDate };
        var metric = new Metric { Expression = requestDto.MetricExpression };
        var dimension = new Dimension { Name = requestDto.DimensionName };


        var reportRequest = new ReportRequest
        {
            DateRanges = new List<DateRange> { dateRange },
            Dimensions = new List<Dimension> { dimension },
            Metrics = new List<Metric> { metric },
            ViewId = requestDto.ViewId
        };

        var getReportsRequest = new GetReportsRequest
        {
            ReportRequests = new List<ReportRequest> { reportRequest }
        };

        try
        {
            var response = await googleServices.ReportingService.Reports.BatchGet(getReportsRequest).ExecuteAsync();
            return Results.Ok(response.Reports.First());
        }
        catch (GoogleApiException e)
        {
            // You can log e.HttpStatusCode or e.Error.Message to get more specific errors
            return Results.Problem("Failed to fetch analytics data: " + e.Error.Message);
        }
        catch (Exception ex)
        {
            return Results.Problem("Failed to fetch analytics data: " + ex.Message);
        }
    }).WithName("FetchAnalyticsReport")
    .WithTags("Google Analytics")
    .AddOpenApiDefaults("Fetch analytics data based on metrics, dimensions, and date range.", "AnalyticsRequestDto");

app.Run();
return;

static string DoIt() => new PasswordGenerator().Generate(64, 10, 10);

public partial class Program
{
}