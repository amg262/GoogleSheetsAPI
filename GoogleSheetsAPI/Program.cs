using System.Net.Mime;
using System.Text.Json;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Docs.v1;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using GoogleSheetsAPI.Data;
using GoogleSheetsAPI.DTOs;
using GoogleSheetsAPI.Helpers;
using GoogleSheetsAPI.Middleware;
using GoogleSheetsAPI.Models;
using GoogleSheetsAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSingleton(s =>
{
    const string json = "ellsworth.json";
    var env = s.GetRequiredService<IWebHostEnvironment>();
    var path = Path.Combine(env.WebRootPath, json);
    var sheetCredential = GoogleCredential.FromFile(path).CreateScoped(SheetsService.Scope.Spreadsheets);
    var docCredential = GoogleCredential.FromFile(path).CreateScoped(DocsService.Scope.Documents);
    var driveCredential = GoogleCredential.FromFile(path).CreateScoped(DriveService.Scope.Drive);

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

    return new GoogleServices(sheetsService, docsService, driveService);
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
app.UseMiddleware<ApiKeyMiddleware>();
app.UseCors("Local");
app.MapHealthCheckEndpoints();

// Define endpoints here.
app.MapPost("api/write", async (GoogleServices googleServices, [FromBody] WriteRequestDto dto) =>
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


app.MapGet("api/read", async (GoogleServices googleServices, [FromBody] ReadRequestDto dto) =>
    {
        var fullRange = $"{dto.Sheetname ?? "Sheet1"}!{dto.Range ?? "A1:Z15"}";
        var request = googleServices.SheetsService.Spreadsheets.Values.Get(dto.SpreadsheetId, fullRange);

        var response = await request.ExecuteAsync();
        return Results.Ok(response.Values);
    }).WithName("ReadData")
    .WithTags("Google Sheets")
    .AddOpenApiDefaults("Read Data from Google Sheets based on request body.", "ReadRequestDto");


app.MapPut("api/update", async (GoogleServices googleServices, [FromBody] WriteRequestDto dto) =>
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


app.MapDelete("api/delete", async (GoogleServices googleServices, [FromBody] WriteRequestDto dto) =>
    {
        var fullRange = $"{dto.Sheetname ?? "Sheet1"}!{dto.Range ?? "A1:Z1"}";
        var request = googleServices.SheetsService.Spreadsheets.Values.Clear(null, dto.SpreadsheetId, fullRange);

        var response = await request.ExecuteAsync();
        return Results.Ok(response.ClearedRange);
    }).WithName("DeleteData")
    .WithTags("Google Sheets")
    .AddOpenApiDefaults("Delete Data in Google Sheets based on request body.", "WriteRequestDto");


app.MapPatch("api/patch", async (GoogleServices googleServices, [FromBody] WriteRequestDto dto) =>
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


app.Run();