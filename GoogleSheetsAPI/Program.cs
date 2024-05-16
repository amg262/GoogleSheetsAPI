using System.Net.Mime;
using System.Text.Json;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using GoogleSheetsAPI.Data;
using GoogleSheetsAPI.DTOs;
using GoogleSheetsAPI.Helpers;
using GoogleSheetsAPI.Middleware;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
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
builder.Services.AddHealthChecks();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "API Key needed to access the endpoints. `x-api-key: YOUR_API_KEY`",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "x-api-key",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                },
                Scheme = "ApiKeyScheme",
                Name = "x-api-key",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Local",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ApiKeyMiddleware>();
app.UseCors("Local");

app.MapHealthChecks("/api/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = async (context, report) =>
    {
        // context.Request.Headers.Append("x-api-key", builder.Configuration.GetValue<string>("ApiKey"));
        var result = JsonSerializer.Serialize(
            new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(entry => new
                {
                    name = entry.Key,
                    status = entry.Value.Status.ToString(),
                    exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                    duration = entry.Value.Duration.ToString(),
                })
            }
        );

        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(result);
    }
});

// This checks if API is live
app.MapHealthChecks("/api/health/live", new HealthCheckOptions
{
    Predicate = _ => false,
});

// Define endpoints here.
app.MapPost("api/write", async (SheetsService sheetsService, [FromBody] WriteRequestDto dto) =>
{
    var fullRange = $"{dto.Sheetname ?? "Sheet1"}!{dto.Range ?? "A1"}";
    var objList = dto.Values.Select(ApiHelper.GetObjectValue).ToList();

    var valueRange = new Google.Apis.Sheets.v4.Data.ValueRange() { Values = new List<IList<object>> { objList } };
    var request = sheetsService.Spreadsheets.Values.Update(valueRange, dto.SpreadsheetId, fullRange);

    request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
    var response = await request.ExecuteAsync();
    return Results.Ok(response.UpdatedRange);
}).WithName("WriteData").WithOpenApi();

app.MapGet("api/read", async (SheetsService sheetsService, [FromBody] ReadRequestDto dto) =>
{
    var fullRange = $"{dto.Sheetname ?? "Sheet1"}!{dto.Range ?? "A1:Z15"}";
    var request = sheetsService.Spreadsheets.Values.Get(dto.SpreadsheetId, fullRange);

    var response = await request.ExecuteAsync();
    return Results.Ok(response.Values);
}).WithName("ReadData").WithOpenApi();

app.MapPut("api/update", async (SheetsService sheetsService, [FromBody] WriteRequestDto dto) =>
{
    var fullRange = $"{dto.Sheetname ?? "Sheet1"}!{dto.Range ?? "A1:Z1"}";
    var objList = dto.Values.Select(ApiHelper.GetObjectValue).ToList();

    var valueRange = new Google.Apis.Sheets.v4.Data.ValueRange() { Values = new List<IList<object>> { objList } };
    var request = sheetsService.Spreadsheets.Values.Update(valueRange, dto.SpreadsheetId, fullRange);

    request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
    var response = await request.ExecuteAsync();
    return Results.Ok(response.UpdatedRange);
}).WithName("UpdateData").WithOpenApi();

app.MapDelete("api/delete", async (SheetsService sheetsService, [FromBody] WriteRequestDto dto) =>
{
    var fullRange = $"{dto.Sheetname ?? "Sheet1"}!{dto.Range ?? "A1:Z1"}";
    var request = sheetsService.Spreadsheets.Values.Clear(null, dto.SpreadsheetId, fullRange);

    var response = await request.ExecuteAsync();
    return Results.Ok(response.ClearedRange);
}).WithName("DeleteData").WithOpenApi();


app.Run();