## Overview

- To run the Music Catalogue UI and the associated Web Service in Docker:
  - Create a folder to contain the key files, UI configuration file, database and Docker Compose file
  - Create a database in that folder using the instructions in the "SQLite Database" section
  - Create a user in the database using the instructions in the "Web Service" section
  - Creste the key files, UI configuration file and Docker Compose file in that folder, as described below

## RapidAPI Key

- To enable album lookup via the AudioDB (see the sections on "Album Lookup" and "Application Configuration File"):
  - Sign up for a RapidAPI key at [RapidAPI](https://rapidapi.com/)
  - Create a text file called "rapidapi.key" containing a single line holding the key
- If the album lookup isn't being used, skip this step
- Note that without a key, any lookup for an album that isn't already held locally will result in an "album not found" error on the search page

## Google Maps API Key

- To use the Retailer Location Maps:
  - Create a Google Maps API key
  - Create a text file called "mapsapi.key" containing a single line holding the key
- If the location maps aren't being used, skip this step
- Note that without a key, the retailer location maps will show a "something went wrong" error

## Create a UI Configuration File

- Create a JSON configuration file for the UI called "ui-config.json":

```json
{
  "api": {
    "baseUrl": "http://localhost:8098"
  },
  "region": {
    "locale": "en-GB",
    "currency": "GBP",
    "geocodingLanguage": "en",
    "geocodingRegion": "GB"
  }
}
```

- Adjust the port number to a free port on the local machine
- Set the locale settings to suit

## Create the Docker Compose file

- The following is an example Docker Compose file that can be used to run the Music Catalogue service and UI from the published Docker images:

```yaml
version: "3.7"

services:
  music-ui:
    container_name: musiccatalogueui
    image: davewalker5/musiccatalogueui:latest
    restart: always
    ports:
      - "8087:3000"
    volumes:
      - C:\MusicCatalogue\ui-config.json:/opt/musiccatalogue.ui-1.28.0.0/config.json

  music-api:
    container_name: musiccatalogueservice
    image: davewalker5/musiccatalogueapisqlite:latest
    restart: always
    ports:
      - "8098:80"
    volumes:
      - C:\MusicCatalogue\:/var/opt/musiccatalogue.api-1.24.0.0/
```

- This assumes that the key and configuration files are in the local folder C:\MusicCatalogue
- This should be changed as appropriate
- To start the application, open a terminal window, change to the folder and run the following command:

```bash
docker compose --project-directory . up -d
```

- The output should look similar to this:

```
C:\MusicCatalogue>docker compose --project-directory . up -d
[+] Building 0.0s (0/0)                                                                                                                                              docker:default
[+] Running 3/3
 ✔ Network musiccatalogue_default   Created                                                                                                                                    0.0s
 ✔ Container musiccatalogueui       Started                                                                                                                                    0.1s
 ✔ Container musiccatalogueservice  Started                                                                                                                                    0.1s
```

- To stop the application, from the same folder run the following command:

```bash
docker compose --project-directory . down
```

- The output should look similar to this:

```
C:\MusicCatalogue>docker compose --project-directory . down
[+] Running 3/3
 ✔ Container musiccatalogueservice  Removed                                                                                                                                    0.4s
 ✔ Container musiccatalogueui       Removed                                                                                                                                   10.5s
 ✔ Network musiccatalogue_default   Removed
```
