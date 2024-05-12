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
    var credentialPath = Path.Combine(env.WebRootPath, "ellsworth.json");
    var credential = GoogleCredential.FromFile(credentialPath).CreateScoped(SheetsService.Scope.Spreadsheets);
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
// app.UseMiddleware<ApiKeyMiddleware>();

// app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

// Define endpoints here.
app.MapPost("/write", (SheetsService sheetsService) =>
{
    string spreadsheetId = "1IETU7EI1UKkVGgaCcoz0R0cnX5tdme-6ealsXvtXR1k";
    string range = "Sheet1!A1:D1";
    var valueRange = new Google.Apis.Sheets.v4.Data.ValueRange()
    {
        Values = new List<IList<object>> { new List<object> { "Data1", "Data2", "Data3", "Data4" } }
    };

    var request = sheetsService.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
    request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
    var response = request.Execute();
    return Results.Ok(response.UpdatedRange);
}).WithName("WriteData").WithOpenApi();


app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}