using Microsoft.Extensions.Options;
using MusicCatalogue.Api.Entities;
using MusicCatalogue.Api.Interfaces;
using MusicCatalogue.Entities.Config;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Reporting;
using MusicCatalogue.Logic.DataExchange.Generic;

namespace MusicCatalogue.Api.Services
{
    public class GenreAlbumsExportService : BackgroundQueueProcessor<GenreAlbumsExportWorkItem>
    {
        private readonly MusicApplicationSettings _settings;
        public GenreAlbumsExportService(
            ILogger<BackgroundQueueProcessor<GenreAlbumsExportWorkItem>> logger,
            IBackgroundQueue<GenreAlbumsExportWorkItem> queue,
            IServiceScopeFactory serviceScopeFactory,
            IOptions<MusicApplicationSettings> settings)
            : base(logger, queue, serviceScopeFactory)
        {
            _settings = settings.Value;
        }

        /// <summary>
        /// Export the albums by genre report
        /// </summary>
        /// <param name="item"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        protected override async Task ProcessWorkItem(GenreAlbumsExportWorkItem item, IMusicCatalogueFactory factory)
        {
            // Get the report data
            MessageLogger.LogInformation("Retrieving the albums by genre report for export");
            var records = await factory.GenreAlbums.GenerateReportAsync(item.GenreId, 1, int.MaxValue);

            // Construct the full path to the export file
            var filePath = Path.Combine(_settings.ReportsExportPath, item.FileName);

            // Export the report
            var exporter = new CsvExporter<GenreAlbum>();
            exporter.Export(records, filePath, ',');
            MessageLogger.LogInformation("Albums by genre report export completed");
        }
    }
}