# MusicCatalogue

[![Build Status](https://github.com/davewalker5/MusicCatalogue/workflows/.NET%20Core%20CI%20Build/badge.svg)](https://github.com/davewalker5/MusicCatalogue/actions)
[![GitHub issues](https://img.shields.io/github/issues/davewalker5/MusicCatalogue)](https://github.com/davewalker5/MusicCatalogue/issues)
[![Coverage Status](https://coveralls.io/repos/github/davewalker5/MusicCatalogue/badge.svg?branch=main)](https://coveralls.io/github/davewalker5/MusicCatalogue?branch=main)
[![Releases](https://img.shields.io/github/v/release/davewalker5/MusicCatalogue.svg?include_prereleases)](https://github.com/davewalker5/MusicCatalogue/releases)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/davewalker5/MusicCatalogue/blob/master/LICENSE)
[![Language](https://img.shields.io/badge/language-c%23-blue.svg)](https://github.com/davewalker5/MusicCatalogue/)
[![Language](https://img.shields.io/badge/language-React-blue.svg)](https://github.com/davewalker5/MusicCatalogue/)
[![Language](https://img.shields.io/badge/database-SQLite-blue.svg)](https://github.com/davewalker5/MusicCatalogue/)
[![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/davewalker5/MusicCatalogue)](https://github.com/davewalker5/MusicCatalogue/)

## Overview

<img src="diagrams/application-schematic.png" alt="Application Schematic" width="600">

- The Music Catalogue repository is contains an application for cataloguing a private music collection
- It supports the following functions:

  - Music catalogue collection browser (artists, albums and tracks)
  - Artist style and mood tagging
  - A "wish list" of albums with the ability to move albums between the main catalogue and the wish list at will
  - An album picker based on genre and artist style parameters
  - A playlist builder:
    - The builder has the following inputs:
      - Playlist "type"
      - Time of day
      - Number of of albums
    - Two broad types of playlists are supported:
      - Normal - maximising breadth while remaining time-of-day aware
      - Curated - constrained to a stylistic neighbourhood but with enough randomness to avoid near-duplicate playlists
    - Playlists may:
      - Be built from the whole catalogue (default)
      - Be built from selected genres
      - Exclude specified genres
  - External API integration for looking up new albums
  - An equipment register browser (equipment, equipment types, manufacturers)
  - A "wish list" of equipment with the ability to move items between the main register and the wish list at will
  - Data import from CSV format files
  - Data export as CSV or Excel workbooks
  - Reports and report export as CSV

- Docker builds of the REST API and GUI are also available

## Getting Started

Please see the [Wiki](https://github.com/davewalker5/MusicCatalogue/wiki) for configuration details and the user guide.

## Authors

- **Dave Walker** - _Initial work_

## Feedback

To file issues or suggestions, please use the [Issues](https://github.com/davewalker5/MusicCatalogue/issues) page for this project on GitHub.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
