using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using GoogleSheetsAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace GoogleSheetsAPI.Extensions;

/// <summary>
/// Static class for setting up services for the WebApplicationBuilder.
/// </summary>
public static class SetupServices
{
    /// <summary>
    /// Adds project-specific services to the WebApplicationBuilder.
    /// </summary>
    /// <param name="builder">The WebApplicationBuilder instance to configure.</param>
    public static void AddProjectServices(this WebApplicationBuilder builder)
    {
        const string serviceAccount = "ellsworth.json";

        builder.Services.AddDbContext<AppDbContext>(opt =>
            opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Configure Google Sheets service with credentials from the specified JSON file
        builder.Services.AddSingleton(s =>
        {
            var env = s.GetRequiredService<IWebHostEnvironment>();
            var path = Path.Combine(env.WebRootPath, serviceAccount);
            var credential = GoogleCredential.FromFile(path).CreateScoped(SheetsService.Scope.Spreadsheets);

            return new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Google Sheets Minimal API",
            });
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Google Sheets API", Version = "v1" });
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: "Local",
                corsPolicyBuilder =>
                {
                    corsPolicyBuilder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowAnyOrigin()
                        .WithOrigins("https://localhost")
                        .AllowCredentials();
                });
        });
    }
}