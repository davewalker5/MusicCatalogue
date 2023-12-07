## About the Music Catalogue

<img src="https://github.com/davewalker5/MusicCatalogue/blob/main/diagrams/application-schematic.png" alt="Application Schematic" width="600">

- The Music Catalogue repository is intended to provide a catalogue for a private music collection
- It supports the following functions:
  - Music catalogue collection browser (artists, albums and tracks)
  - A "wish list" of albums with the ability to move albums between the main catalogue and the wish list at will
  - Album search
  - External API integration for looking up new albums
  - An equipment register browser (equipment, equipment types, manufacturers)
  - A "wish list" of equipment with the ability to move items between the main register and the wish list at will
  - Data import from CSV format files
  - Data export as CSV or Excel workbooks
  - Reports and report export as CSV
- It contains the following components:

| Component    | Language | Purpose                                                            |
| ------------ | -------- | ------------------------------------------------------------------ |
| Entities     | C#       | Catalogue entities (albums, artists, tracks)                       |
| Data         | C#       | Database context and migrations for a SQLite database              |
| Logic        | C#       | Business logic for browsing the data and external API integration  |
| Console Tool | C#       | Command line tool providing facilities based on the business logic |
| REST API     | C#       | Web API exposing the facilities provided by the business logic     |
| GUI          | React.js | Browser UI for catalogue browsing                                  |

- NuGet packages are available for the entities, data and logic
- Docker builds of the REST API and GUI are also available
