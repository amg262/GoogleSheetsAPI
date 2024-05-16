// using System.Net.Mime;
// using System.Text.Json;
// using Google.Apis.Sheets.v4;
// using GoogleSheetsAPI.DTOs;
// using GoogleSheetsAPI.Middleware;
// using Microsoft.AspNetCore.Diagnostics.HealthChecks;
// using Microsoft.AspNetCore.Mvc;
//
// namespace GoogleSheetsAPI.Extensions;
//
// /// <summary>
// /// Static class for configuring the pipeline and endpoints of the WebApplication.
// /// </summary>
// public static class ConfigurePipeline
// {
//     /// <summary>
//     /// Configures the application's endpoints and middleware.
//     /// </summary>
//     /// <param name="app">The WebApplication instance to configure.</param>
//     public static async void UseProjectEndpoints(this WebApplication app)
//     {
//         // Enable Swagger UI in the development environment.
//         if (app.Environment.IsDevelopment())
//         {
//             app.UseSwagger();
//             app.UseSwaggerUI();
//         }
//
//         // Use custom API key middleware for request authentication.
//         app.UseMiddleware<ApiKeyMiddleware>();
//         app.UseHttpsRedirection(); // Uncomment if HTTPS redirection is needed
//
//         // This checks if the db is ready to accept requests and gives a response
//         app.MapHealthChecks("/api/health/ready", new HealthCheckOptions
//         {
//             Predicate = check => check.Tags.Contains("ready"),
//             ResponseWriter = async (context, report) =>
//             {
//                 var result = JsonSerializer.Serialize(
//                     new
//                     {
//                         status = report.Status.ToString(),
//                         checks = report.Entries.Select(entry => new
//                         {
//                             name = entry.Key,
//                             status = entry.Value.Status.ToString(),
//                             exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
//                             duration = entry.Value.Duration.ToString()
//                         })
//                     }
//                 );
//
//                 context.Response.ContentType = MediaTypeNames.Application.Json;
//                 await context.Response.WriteAsync(result);
//             }
//         });
//
//         // This checks if API is live
//         app.MapHealthChecks("/api/health/live", new HealthCheckOptions
//         {
//             Predicate = _ => false,
//         });
//
//         
//         await ConfigureApi(app);
//     }
//
//     private static async Task ConfigureApi(WebApplication app)
//     {
//         // Define endpoints here.
//         app.MapPost("/api/write", async (SheetsService sheetsService, [FromBody] WriteRequestDto dto) =>
//         {
//             var fullRange = $"{dto.Sheetname ?? "Sheet1"}!{dto.Range ?? "A1"}";
//             var objList = dto.Values.Select(GetObjectValue).ToList();
//             var valueRange = new Google.Apis.Sheets.v4.Data.ValueRange { Values = new List<IList<object>> { objList } };
//             var request = sheetsService.Spreadsheets.Values.Update(valueRange, dto.SpreadsheetId, fullRange);
//             request.ValueInputOption =
//                 SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
//             var response = await request.ExecuteAsync();
//             return Results.Ok(response.UpdatedRange);
//         }).WithName("WriteData").WithOpenApi();
//
//         app.MapGet("/api/read", async (SheetsService sheetsService) =>
//         {
//             string spreadsheetId = "1IETU7EI1UKkVGgaCcoz0R0cnX5tdme-6ealsXvtXR1k";
//             string range = "Sheet1!A1:D1";
//             var request = sheetsService.Spreadsheets.Values.Get(spreadsheetId, range);
//             var response = await request.ExecuteAsync();
//             return Results.Ok(response.Values);
//         }).WithName("ReadData").WithOpenApi();
//     }
//
//     /// <summary>
//     /// Converts a JsonElement object to an appropriate .NET object type.
//     /// </summary>
//     /// <param name="obj">The object to convert, typically a JsonElement.</param>
//     /// <returns>The converted object as a .NET type.</returns>
//     private static object GetObjectValue(object? obj)
//     {
//         if (obj == null) return "NULL";
//
//         var typeOfObject = ((JsonElement)obj).ValueKind;
//
//         return typeOfObject switch
//         {
//             JsonValueKind.Number => float.Parse(obj.ToString()), // return long.Parse(obj.ToString());
//             JsonValueKind.String => obj.ToString(),
//             JsonValueKind.True => true,
//             JsonValueKind.False => false,
//             JsonValueKind.Null => null,
//             JsonValueKind.Undefined => null,
//             JsonValueKind.Object => obj.ToString(),
//             JsonValueKind.Array => obj.ToString(),
//             _ => obj.ToString()
//         };
//     }
// }