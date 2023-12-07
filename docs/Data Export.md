## Data Export

- To export the music catalogue, click on the "Export" menu bar item:

<img src="https://github.com/davewalker5/MusicCatalogue/blob/main/diagrams/catalogue-export.png" alt="Music Catalogue Export" width="600">

- Enter the file name, without a path, and click on the "Export" button to request an export
- A request is sent to the web service to perform an export of the catalogue in the background
- The "export" page is updated when the request has been sent:

<img src="https://github.com/davewalker5/MusicCatalogue/blob/main/diagrams/catalogue-export-requested.png" alt="Music Catalogue Export" width="600">

- Once the export is complete, the file will appear in the folder indicated by the "CatalogueExportPath" configuration setting (see below)
- The exported format is based on the file extension for the supplied file path, as per the command-line tool (see above)
