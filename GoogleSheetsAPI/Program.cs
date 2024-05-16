using GoogleSheetsAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddProjectServices();

var app = builder.Build();

app.UseProjectEndpoints();

app.Run();