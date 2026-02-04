using Microsoft.Extensions.Options;
using MusicCatalogue.Api.Entities;
using MusicCatalogue.Api.Interfaces;
using MusicCatalogue.Entities.Config;
using MusicCatalogue.Entities.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Api.Services
{
    [ExcludeFromCodeCoverage]
    public class PlaylistExportService : BackgroundQueueProcessor<PlaylistExportWorkItem>
    {
        private readonly MusicApplicationSettings _settings;

        public PlaylistExportService(
            ILogger<BackgroundQueueProcessor<PlaylistExportWorkItem>> logger,
            IBackgroundQueue<PlaylistExportWorkItem> queue,
            IServiceScopeFactory serviceScopeFactory,
            IOptions<MusicApplicationSettings> settings)
            : base(logger, queue, serviceScopeFactory)
        {
            _settings = settings.Value;
        }

        /// <summary>
        /// Export a playlist
        /// </summary>
        /// <param name="item"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        protected override async Task ProcessWorkItem(PlaylistExportWorkItem item, IMusicCatalogueFactory factory)
        {
            MessageLogger.LogInformation("Playlist export started");

            // Use the file extension to determine which exporter to use
            var extension = Path.GetExtension(item.FileName).ToLower();
            IPlaylistExporter? exporter = extension == ".xlsx" ? factory.PlaylistXlsxExporter : factory.PlaylistCsvExporter;

            // Construct the full path to the export file
            var filePath = Path.Combine(_settings.CatalogueExportPath, item.FileName);

            // Export the playlist
            exporter.Export(filePath, item.Playlist ?? new());
            MessageLogger.LogInformation("Playlist export completed");
        }
    }
}