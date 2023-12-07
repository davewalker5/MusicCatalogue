## Overview

- The console application provides a simple command line interface for:
  - Looking up albums one at a time, given an artist and album title
  - Importing data from CSV files
  - Exporting data to CSV files or Excel workbooks
- The album lookup facility uses the algorithm described under "Album Lookup", below
- Consequently, searching for an album that's not currently in the catalogue will add it to the local database
- The console application doesn't use the web service (see below) and can be used standalone

## Configuration

- The console application uses an "appsettings.json" file to hold configuration settings
- It's described in the "Application Configuration File" section, below

## Command Line Options

- The following command line arguments are supported:

| Option   | Short Name | Required Values                  | Comments                                    |
| -------- | ---------- | -------------------------------- | ------------------------------------------- |
| --lookup | -l         | Artist name, album title, target | Performs an album lookup                    |
| --import | -i         | CSV file path                    | Import data from the specified CSV file     |
| --export | -e         | File path                        | Export the collection to the specified file |

- For album lookups
  - It is recommended that the artist name and album title are both double-quoted
  - This is mandatory if either contains spaces
  - The target determines whether the album is tagged as being in the wish list or the main catalogue:

| Value     | Target                                 |
| --------- | -------------------------------------- |
| wishlist  | Store new albums in the wish list      |
| catalogue | Store new albums in the main catalogue |

- For data exports, the exported format is based on the file extension for the supplied file path:

| Extension | Exported format |
| --------- | --------------- |
| xlsx      | Excel workbook  |
| csv       | CSV file        |

## CSV File Format

- The first row in the CSV file is expected to contain headers and is ignored
- Durations should be specified in MM:SS format, e.g. "03:10" for 3 minutes and 10 seconds
- The following is an example, illustrating the format for the headers and for the rows containing data:

```
Artist,Album,Genre,Released,Cover Url,Track Number,Track,Duration,Wish List,Purchase Date,Price,Retailer
George Harrison,All Things Must Pass,Rock & Roll,1970,https://www.theaudiodb.com/images/media/album/thumb/all-things-must-pass-4f09954aa6370.jpg,1,I'd Have You Anytime,02:56,False,12/11/2023,59.07,Amazon
```

- Exports include all albums in both the main catalogue and the wish list

## Example - Album Lookup

- The following command will look-up the album "Blue Train" by John Coltrane
- In this example, if the album isn't stored locally and is looked up using the external APIs (see below) the results will be stored in the main cataloge

```bash
MusicCatalogue.LookupTool --lookup "John Coltrane" "Blue Train" catalogue
```

- The output lists the album details and the number, title and duration of each track:

![Console Lookup Tool](https://github.com/davewalker5/MusicCatalogue/blob/main/diagrams/lookup-tool.png)
