FROM mcr.microsoft.com/dotnet/core/aspnet:latest
ARG APPLICATION_VERSION
COPY musiccatalogue.api-${APPLICATION_VERSION} /opt/musiccatalogue.api
WORKDIR /opt/musiccatalogue.api/bin
ENTRYPOINT [ "./MusicCatalogue.Api" ]
