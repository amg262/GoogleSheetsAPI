using System.Text.Json;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using GoogleSheetsAPI.Data;
using GoogleSheetsAPI.DTOs;
using GoogleSheetsAPI.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSingleton(s =>
{
    var env = s.GetRequiredService<IWebHostEnvironment>();
    var path = Path.Combine(env.WebRootPath, "ellsworth.json");
    var credential = GoogleCredential.FromFile(path).CreateScoped(SheetsService.Scope.Spreadsheets);

    return new SheetsService(new BaseClientService.Initializer()
    {
        HttpClientInitializer = credential,
        ApplicationName = "Google Sheets Minimal API",
    });
});
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ApiKeyMiddleware>();

// app.UseHttpsRedirection();

// Define endpoints here.
app.MapPost("/api/write", async (SheetsService sheetsService, [FromBody] WriteRequestDto dto) =>
{
    // string spreadsheetId2 = "1IETU7EI1UKkVGgaCcoz0R0cnX5tdme-6ealsXvtXR1k";
    // string range2 = "Sheet1!A1:D1";
    var fullRange = $"{dto.Sheetname ?? "Sheet1"}!{dto.Range ?? "A1"}";

    var objList = dto.Values.Select(GetObjectValue).ToList();

    var valueRange = new Google.Apis.Sheets.v4.Data.ValueRange() { Values = new List<IList<object>> { objList } };

    var request = sheetsService.Spreadsheets.Values.Update(valueRange, dto.SpreadsheetId, fullRange);

    request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
    var response = await request.ExecuteAsync();
    return Results.Ok(response.UpdatedRange);
}).WithName("WriteData").WithOpenApi();

app.MapGet("/api/read", async (SheetsService sheetsService) =>
{
    string spreadsheetId = "1IETU7EI1UKkVGgaCcoz0R0cnX5tdme-6ealsXvtXR1k";
    string range = "Sheet1!A1:D1";
    var request = sheetsService.Spreadsheets.Values.Get(spreadsheetId, range);
    var response = await request.ExecuteAsync();
    return Results.Ok(response.Values);
}).WithName("ReadData").WithOpenApi();


app.Run();
return;

static object GetObjectValue(object? obj)
{
    if (obj == null) return "null";

    var typeOfObject = ((JsonElement)obj).ValueKind;

    return typeOfObject switch
    {
        JsonValueKind.Number => float.Parse(obj.ToString()), // return long.Parse(obj.ToString());
        JsonValueKind.String => obj.ToString(),
        JsonValueKind.True => true,
        JsonValueKind.False => false,
        JsonValueKind.Null => null,
        JsonValueKind.Undefined => null,
        JsonValueKind.Object => obj.ToString(),
        JsonValueKind.Array => obj.ToString(),
        _ => obj.ToString()
    };
}