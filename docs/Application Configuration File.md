- The console application and web service use a common configuration file format, described in this section
- The GUI has a much simpler configuration file that's described along with the UI, above

## General Settings and Database Connection String

- The appsettings.json file in the console application project contains the following keys for controlling the application:

| Section             | Key                 | Purpose                                                                 |
| ------------------- | ------------------- | ----------------------------------------------------------------------- |
| ApplicationSettings | LogFile             | Path and name of the log file. If this is blank, no log file is created |
| ApplicationSettings | MinimumLogLevel     | Minimum message severity to log (Debug, Info, Warning or Error)         |
| ApplicationSettings | CatalogueExportPath | Path to the folder where music catalogue exports are written            |
| ApplicationSettings | ReportsExportPath   | Path to the folder where mreport exports are written                    |
| ApplicationSettings | ApiEndpoints        | Set of endpoint definitions for external APIs                           |
| ApplicationSettings | ApiServiceKeys      | Set of API key definitions for external APIs                            |
| ConnectionStrings   | MusicCatalogueDB    | SQLite connection string for the database                               |

## External API Configuration

- The lookup tool and web service include integration with the TheAudioDB public API for artist, album and track details lookup:

[TheAudioDB](https://rapidapi.com/theaudiodb/api/theaudiodb)

- To use the integration, a RapidAPI subscription is needed, as this includes an API key needed to acces the APIs
- Signup is free, but daily free usage is restricted with a nominal charge being made for requests above the free limit
- The integration is configured via the following keys in the configuration file:

| Section             | Sub-Section    | Purpose                                                                                     |
| ------------------- | -------------- | ------------------------------------------------------------------------------------------- |
| ApplicationSettings | ApiEndpoints   | A list of endpoint definitions, each containing the endpoint type, service and endpoint URL |
| ApplicationSettings | ApiServiceKeys | A list of entries mapping each service to the API key needed to access that service         |

### ApiEndpoint Definitions

- An example API endpoint definition is shown below:

```json
{
  "EndpointType": "Albums",
  "Service": "TheAudioDB",
  "Url": "https://theaudiodb.p.rapidapi.com/searchalbum.php"
}
```

- Possible values for the endpoint type are:

| Type   | Description                                                                               |
| ------ | ----------------------------------------------------------------------------------------- |
| Albums | Endpoint used to retrieve album details given an artist name and album title              |
| Tracks | Endpoint used to retrieve track details given an album ID returned by the albums endpoint |

- Currently, only the TheAudioDB APIs are supported

### ApiServiceKey Definitions

- An example key definition for a service is shown below:

```json
{
  "Service": "TheAudioDB",
  "Key": "put-your-RapidPI-key-here"
}
```

- The "Key" can also specify an external text file in an alternative location containing a single line holding the API key
- This approach is used in the Docker image of the API, where the key file is mounted from the host, avoiding baking the API key into the image
