using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using GoogleSheetsAPI.Data;
using GoogleSheetsAPI.Middleware;
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
    var credential = GoogleCredential.FromFile(path)
        .CreateScoped(SheetsService.Scope.Spreadsheets);

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
app.MapPost("/write",
    async (SheetsService sheetsService, string? spreadsheetId, string? sheetname, string? range,
        List<IList<object>?>? values) =>
    {
        // string spreadsheetId2 = "1IETU7EI1UKkVGgaCcoz0R0cnX5tdme-6ealsXvtXR1k";
        // string range2 = "Sheet1!A1:D1";
        var fullRange = $"{sheetname ?? "Sheet1"}!{range ?? "A1:Z1"}";

        var valueRange = new Google.Apis.Sheets.v4.Data.ValueRange()
        {
            Values = values ?? [new List<object> { "Ground Contorl", "to major tom", "Oh hello", "Test again" }]
        };


        // var valueRange = new Google.Apis.Sheets.v4.Data.ValueRange()
        // {
        //     Values = new List<IList<object>>
        //         { new List<object> { "Ground Contorl", "to major tom", "Oh hello", "Test again" } }
        // };

        var request = sheetsService.Spreadsheets.Values.Update(valueRange,
            spreadsheetId ?? "1IETU7EI1UKkVGgaCcoz0R0cnX5tdme-6ealsXvtXR1k", fullRange);

        request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
        var response = await request.ExecuteAsync();
        return Results.Ok(response.UpdatedRange);
    }).WithName("WriteData").WithOpenApi();

app.MapGet("/read", async (SheetsService sheetsService) =>
{
    string spreadsheetId = "1IETU7EI1UKkVGgaCcoz0R0cnX5tdme-6ealsXvtXR1k";
    string range = "Sheet1!A1:D1";
    var request = sheetsService.Spreadsheets.Values.Get(spreadsheetId, range);
    var response = await request.ExecuteAsync();
    return Results.Ok(response.Values);
}).WithName("ReadData").WithOpenApi();


app.Run();