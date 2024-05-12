FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 8080
EXPOSE 8081

COPY GoogleSheetsAPI/GoogleSheetsAPI.csproj GoogleSheetsAPI/GoogleSheetsAPI.csproj

RUN dotnet restore GoogleSheetsAPI/GoogleSheetsAPI.csproj

# Copy the app folders over
COPY . .
WORKDIR /app/GoogleSheetsAPI
RUN dotnet publish -c Release -o /app/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet", "GoogleSheetsAPI.dll" ]