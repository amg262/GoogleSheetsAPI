﻿using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

namespace GoogleSheetsAPI.Extensions;

public static class OpenApiExtensions
{
    public static IServiceCollection AddSwaggerMetadata(this IServiceCollection services)
    {
        services.AddHealthChecks();
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Google Sheets API",
                Version = "v1",
                Description =
                    "This API allows server-to-server interactions with Google Sheets. It provides endpoints to " +
                    "perform CRUD (Create, Read, Update, Delete) operations on Google Sheets. The API is designed " +
                    "for automated, backend processes and requires an API key for authentication. All endpoints " +
                    "require an API key to be passed in the request headers. " +
                    "The API key should be included in the `x-api-key` header.",
                Contact = new OpenApiContact
                {
                    Name = "Andrew",
                    Email = "agunn@ellsworth.com",
                    // Url = new Uri("https://ellsworth.com")
                }
            });

            options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Description = "API Key needed to access the endpoints. `x-api-key: YOUR_API_KEY`",
                In = ParameterLocation.Header,
                Name = "x-api-key",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "ApiKeyScheme"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme, Id = "ApiKey"
                        },
                        Scheme = "ApiKeyScheme",
                        Name = "x-api-key",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });
        return services;
    }

    public static RouteHandlerBuilder AddOpenApiDefaults(this RouteHandlerBuilder builder, string? description,
        string schemaId)
    {
        return builder.WithOpenApi(operation =>
        {
            operation.Responses.TryAdd("200", new OpenApiResponse { Description = "Successful Operation" });
            operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.TryAdd("500", new OpenApiResponse { Description = "Internal Server Error" });
            operation.Description = description;
            operation.RequestBody = new OpenApiRequestBody
            {
                Required = true,
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.Schema,
                                Id = schemaId
                            }
                        }
                    }
                }
            };
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new()
                {
                    [new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "ApiKey"
                        }
                    }] = new List<string>()
                }
            };
            return operation;
        });
    }

    public static IEndpointConventionBuilder AddOpenApiHealthCheckDefaults(this IEndpointConventionBuilder builder,
        string? description)
    {
        return builder.WithMetadata(new OpenApiOperation
        {
            Responses = new OpenApiResponses
            {
                { "200", new OpenApiResponse { Description = "Healthy" } },
                { "503", new OpenApiResponse { Description = "Unhealthy" } }
            },
            Description = description,
        });
    }

    public static IEndpointRouteBuilder MapHealthCheckEndpoints(this IEndpointRouteBuilder endpoints,
        IConfiguration configuration)
    {
        endpoints.MapHealthChecks("/api/health/ready", new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains("ready"),
                ResponseWriter = async (context, report) =>
                {
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
            }).WithName("HealthReady")
            .WithTags("Health Checks")
            .AddOpenApiHealthCheckDefaults("Health check to indicate if the service is ready and accepting requests.");

        endpoints.MapHealthChecks("/api/health/live", new HealthCheckOptions
            {
                Predicate = _ => false,
            }).WithName("HealthLive")
            .WithTags("Health Checks")
            .AddOpenApiHealthCheckDefaults("Health check to indicate if the service is live and accepting requests.");

        return endpoints;
    }
}