## Overview

- To search for an album, click on the "Search" menu bar item:

<img src="https://github.com/davewalker5/MusicCatalogue/blob/main/diagrams/album-search.png" alt="Album Search" width="600">

- Enter the artist name and album title and select the catalogue to save the album to, either the main catalogue or the wish list
- Click on "Lookup" to search for the album
- If the album is found, the track list for the album is displayed
- The album lookup facility uses the algorithm described under "Album Lookup"
- Consequently, searching for an album that's not currently in the catalogue will add it to the local database

## Algorithm

- The local SQLite database is searched preferentially for album details
- The external APIs are only used if an artist and/or album aren't found locally
- Details returned by the external APIs are stored in the local database provided the returned data is complete:
  - The artist is found
  - The album is found
  - The album has at least one track associated with it
- Consequently, subsequent searches with the same criteria will return data from the local database, not the APIs
- Artist names, album titles and track names are stored in title case
- Searches convert the search criteria to title case when looking details up in the database
