## Facilities

- The REST Web Service implements endpoints for
  - Authenticating registered users
  - Retrieving artist details from the local database
  - Retrieving album and track details from the local database
  - Looking up albums via the external APIs (see below)
  - Managing retailers and purchase details
- The external lookup uses the "album lookup" algorithm described under "Album Lookup", below
- Swagger documentation exposed at:

```
/swagger/index.html
```

- For full details of the service endpoints, it's recommended to build and run the service and review the documentation exposed at the above URL

## Configuration

- The web service uses an "appsettings.json" file to hold configuration settings
- It's described in the "Application Configuration File" section, below

## Authentication

- The service uses bearer token authentication, so clients should:
  - Use the /users/authenticate endpoint to get a token
  - Set the authorization header in subsequent requests:

```
Authorization: Bearer <token>
```

## Database Users

- To authenticate, users must have a record in the USERS table of the database associating a username with their hashed password
- The following is a code snippet for adding a user to the database:

```csharp
var userName = "SomeUser";
var password = "ThePassword";
var context = new MusicCatalogueDbContextFactory().CreateDbContext(Array.Empty<string>());
var factory = new MusicCatalogueFactory(context);
Task.Run(() => factory.Users.AddAsync(userName, password)).Wait();
```
