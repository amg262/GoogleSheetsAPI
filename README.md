# Google Sheets API Project

This project is a minimal API built with .NET 8.0 that interacts with Google Sheets using Google's Sheets API v4. It provides endpoints to read and write data to a Google Sheet.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

- .NET 8.0 SDK
- Docker (optional)

### Installing

1. Clone the repository to your local machine.
2. Navigate to the project directory.
3. Run `dotnet restore` to restore the project dependencies.

### Configuration

The project uses a Google service account for authentication with the Google Sheets API. You need to download a JSON key file for your service account and place it in the `wwwroot` directory of the project. The name of this file should be `ellsworth.json`.

The project also uses an API key for authentication. This key is specified in the `appsettings.json` file.

## Running the Application

You can run the application using the .NET CLI:

```bash
dotnet run
```

The application will start and listen on `http://localhost:8001`.

## API Endpoints

The application provides the following endpoints:

- `POST /api/write`: Writes data to a Google Sheet. The request body should be a JSON object that specifies the spreadsheet ID, the sheet name, the range, and the values to write.

- `GET /api/read`: Reads data from a Google Sheet. The spreadsheet ID and the range are hardcoded in the application.

## Built With

- [.NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Google Sheets API v4](https://developers.google.com/sheets/api)

## Authors

- amg262

## License

This project is licensed under the MIT License.