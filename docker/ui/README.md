# musiccatalogueui

The [MusicCatalogue](https://github.com/davewalker5/MusicCatalogue) GitHub project implements the entities, business logic, a REST service and a UI for a SQL-based personal music collection catalogue, providing facilities for recording and querying the following data:

- Artist
- Albums
- Tracks

The musiccatalogueui image contains a build of the UI.

## Getting Started

### Prerequisities

In order to run this image you'll need docker installed.

- [Windows](https://docs.docker.com/windows/started)
- [OS X](https://docs.docker.com/mac/started/)
- [Linux](https://docs.docker.com/linux/started/)

### Usage

#### Container Parameters

The following "docker run" parameters are recommended when running the musiccatalogueui image:

| Parameter | Value                                                               | Purpose                                                   |
| --------- | ------------------------------------------------------------------- | --------------------------------------------------------- |
| -d        | -                                                                   | Run as a background process                               |
| -v        | /local/config/ui-config.json:/var/opt/musiccatalogue.ui/config.json | Mount the file containing the UI config                   |
| -p        | 8086:3000                                                           | Expose the container's port 3000 as port 8086 on the host |
| --rm      | -                                                                   | Remove the container automatically when it stops          |

For example:

```shell
docker run -d -v /local/config/ui-config.json:/var/opt/musiccatalogue.ui/config.json -p 8086:3000 --rm  davewalker5/musiccatalogueui:latest
```

The local path given to the -v argument is described, below, and should be replaced with a value appropriate for the host running the container.

Similarly, the port number "8086" can be replaced with any available port on the host.

#### Mounted Configuration File

The description of the container parameters, above, specifies that a file containing the UI configuration file is mounted, using the "-v" parameter.

The file should contain a single key, holding the base URL for the associated
web service:

```json
{
  "api": {
    "baseUrl": "http://localhost:8098"
  }
}
```

The UI is a React application that runs in the local browser, so the URL must be
accessible from the host.

#### Running the Image

As the UI needs to be run in concert with the web service, it's recommended to run
it using Docker Compose. The following is an example compose file:

```yml
services:
  music-ui:
    container_name: musiccatalogueui
    image: davewalker5/musiccatalogueui:latest
    restart: always
    ports:
      - "8086:3000"
    volumes:
      - /local/config/ui-config.json:/opt/musiccatalogue.ui/config.json

  music-api:
    container_name: musiccatalogueservice
    image: davewalker5/musiccatalogueapisqlite:latest
    restart: always
    ports:
      - "8098:80"
    volumes:
      - /local/data:/var/opt/musiccatalogue.api/
```

The local path to the config file and the local data path and port for the service should be set per the
instructions, above, and the instructions for the [musiccatalogueapisqlite](https://hub.docker.com/repository/docker/davewalker5/musiccatalogueapisqlite/) image.

The port for the UI, 8086, should be set to an available local port.

## Find Us

- [MusicCatalogue on GitHub](https://github.com/davewalker5/MusicCatalogue)

## Versioning

For the versions available, see the [tags on this repository](https://github.com/davewalker5/MusicCatalogue/tags).

## Authors

- **Dave Walker** - _Initial work_ - [LinkedIn](https://www.linkedin.com/in/davewalker5/)

See also the list of [contributors](https://github.com/davewalker5/MusicCatalogue.Api/contributors) who
participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/davewalker5/MusicCatalogue/blob/master/LICENSE) file for details.
