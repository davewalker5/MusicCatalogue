# musiccatalogueapisqlite

The [MusicCatalogue](https://github.com/davewalker5/MusicCatalogue) GitHub project implements the entities, business logic and a REST service for a SQL-based personal music collection catalogue, providing facilities for recording and querying the following data:

- Artist
- Albums
- Tracks

The musiccatalogueapisqlite image contains a build of the logic and REST service for a SQLite database.

## Getting Started

### Prerequisities

In order to run this image you'll need docker installed.

- [Windows](https://docs.docker.com/windows/started)
- [OS X](https://docs.docker.com/mac/started/)
- [Linux](https://docs.docker.com/linux/started/)

### Usage

#### Container Parameters

The following "docker run" parameters are recommended when running the flightrecorderapisqlite image:

| Parameter | Value                                      | Purpose                                                               |
| --------- | ------------------------------------------ | --------------------------------------------------------------------- |
| -d        | -                                          | Run as a background process                                           |
| -v        | /local:/var/opt/musiccatalogue.api-1.0.0.0 | Mount the host folder containing the SQLite database and API key file |
| -p        | 7295:80                                    | Expose the container's port 80 as port 7295 on the host               |
| --rm      | -                                          | Remove the container automatically when it stops                      |

For example:

```shell
docker run -d -v  /local:/var/opt/musiccatalogue.api-1.0.0.0 -p 7295:80 --rm  davewalker5/musiccatalogueapisqlite:latest
```

The "/local" path given to the -v argument is described, below, and should be replaced with a value appropriate for the host running the container.

The version (1.0.0.0) should also be replaced with the version number for the version of the image being run.

Similarly, the port number "7295" can be replaced with any available port on the host.

#### Volumes

The description of the container parameters, above, specifies that a folder containing the SQLite database file and the external API key file is mounted in the running container, using the "-v" parameter.

That folder should contain a SQLite database that has been created using the instructions in the [Music Catalogue README](https://github.com/davewalker5/MusicCatalogue).

The folder containing the "musiccatalogue.db" file can then be passed to the "docker run" command using the "-v" parameter.

#### Running the Image

To run the image, enter the following command, substituting "/local" for the host folder containing the SQLite database and the version (1.0.0.0) with the version number for the version of the image being run, as described:

```shell
docker run -d -v  /local:/var/opt/musiccatalogue.api-1.0.0.0 -p 7295:80 --rm  davewalker5/musiccatalogueapisqlite:latest
```

Once the container is running, browse to the following URL on the host:

http://localhost:7295

You should see the Swagger API documentation for the API.

## Built With

The musiccatalogueapisqlite image was been built with the following:

| Aspect         | Version         |
| -------------- | --------------- |
| .NET           | 7.0.401         |
| Target Runtime | linux-x64       |
| Docker Desktop | 4.23.0 (120376) |

## Find Us

- [MusicCatalogue on GitHub](https://github.com/davewalker5/MusicCatalogue)

## Versioning

For the versions available, see the [tags on this repository](https://github.com/davewalker5/MusicCatalogue/tags).

## Authors

- **Dave Walker** - _Initial work_ - [LinkedIn](https://github.com/)

See also the list of [contributors](https://github.com/davewalker5/MusicCatalogue.Api/contributors) who
participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/davewalker5/MusicCatalogue/blob/master/LICENSE) file for details.
