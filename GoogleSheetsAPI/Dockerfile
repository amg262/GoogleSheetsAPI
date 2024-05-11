FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GoogleSheetsAPI/GoogleSheetsAPI.csproj", "GoogleSheetsAPI/"]
RUN dotnet restore "GoogleSheetsAPI/GoogleSheetsAPI.csproj"
COPY . .
WORKDIR "/src/GoogleSheetsAPI"
RUN dotnet build "GoogleSheetsAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "GoogleSheetsAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GoogleSheetsAPI.dll"]
