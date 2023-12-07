## Database Schema

![Database Schema](https://github.com/davewalker5/MusicCatalogue/blob/main/diagrams/database-schema.png)

- Note that the "Duration" field on the TRACKS table denotes the track duration in ms

## Database Management

- The application uses Entity Framework Core and initial creation and management of the database is achieved using EF Core database migrations
- To create the database for the first time, first install the .NET Core SDK and then install the "dotnet ef" tool:

```bash
dotnet tool install --global dotnet-ef
```

- Update the database path in the "appsettings.json" file in the terminal application project to point to the required database location
- Build the solution
- Open a terminal window and change to the MusicCatalogue.Data project
- Run the following command, making sure to use the path separator appropriate for your OS:

```bash
dotnet ef database update -s ../MusicCatalogue.LookupTool/MusicCatalogue.LookupTool.csproj
```

- If the database doesn't exist, it will create it
- It will then bring the database up to date by applying all pending migrations
