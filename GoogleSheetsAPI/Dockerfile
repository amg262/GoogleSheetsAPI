#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER $APP_UID
#WORKDIR /app
#EXPOSE 80
#EXPOSE 8080
#EXPOSE 8081
#
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY ["GoogleSheetsAPI/GoogleSheetsAPI.csproj", "GoogleSheetsAPI/"]
#RUN dotnet restore "GoogleSheetsAPI/GoogleSheetsAPI.csproj"
#COPY . .
#WORKDIR "/src/GoogleSheetsAPI"
#RUN dotnet build "GoogleSheetsAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "GoogleSheetsAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "GoogleSheetsAPI.dll"]


FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 8080
EXPOSE 8081

# copy all .csproj files and restore as distinct layers.   Use of the same COPY command
# for every dockerfile in the project to take advantage of docker caching
#COPY ../../GoogleSheetsAPI.sln ../../GoogleSheetsAPI.sln
#COPY GoogleSheetsAPI.sln GoogleSheetsAPI.sln
COPY GoogleSheetsAPI/GoogleSheetsAPI.csproj GoogleSheetsAPI/GoogleSheetsAPI.csproj

# Restore package deps
#RUN dotnet restore /app/GoogleSheetsAPI.sln
RUN dotnet restore "GoogleSheetsAPI/GoogleSheetsAPI.csproj"

# Copy the app folders over
COPY . .
WORKDIR /app/GoogleSheetsAPI
RUN dotnet publish -c Release -o /app/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet", "GoogleSheetsAPI.dll" ]