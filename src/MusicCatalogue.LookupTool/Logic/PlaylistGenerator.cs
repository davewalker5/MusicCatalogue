using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Playlists;
using Spectre.Console;

namespace MusicCatalogue.LookupTool.Logic
{
    internal class PlaylistGenerator
    {
        private readonly IMusicCatalogueFactory _factory;

        public PlaylistGenerator(IMusicCatalogueFactory factory)
            => _factory = factory;

        /// <summary>
        /// Generate a playlist and show it on the console
        /// </summary>
        /// <param name="type"></param>
        /// <param name="timeOfDay"></param>
        /// <param name="numberOfEntries"></param>
        /// <returns></returns>
        public async Task GeneratePlaylistAsync(PlaylistType type, TimeOfDay timeOfDay, int numberOfEntries)
        {
            // Create a playlist
            var playlist = await _factory.PlaylistBuilder.BuildPlaylistAsync(type, timeOfDay, numberOfEntries, [], []);

            // Create a table and add the columns to it
            var table = new Table();
            table.AddColumn("#");
            table.AddColumn("Artist");
            table.AddColumn("Album");
            table.AddColumn("Playing Time");

            // Add the albums to the table
            for (int i = 0; i < playlist.Albums.Count; i++)
            {
                var rowData = new string[] {
                    GetCellData(i + 1),
                    GetCellData(playlist.Albums[i].Artist!.Name),
                    GetCellData(playlist.Albums[i].Title),
                    GetCellData(playlist.Albums[i].FormattedPlayingTime)
                };

                table.AddRow(rowData);
            }

            // Add the total playing time row
            var totalPlayingTime = playlist.FormattedPlayingTime;
            var totalPlayingTimeRow = new string[] {
                    GetCellData(""),
                    GetCellData(""),
                    GetCellData(""),
                    GetCellData(totalPlayingTime)
                };
            table.AddRow(totalPlayingTimeRow);

            AnsiConsole.Write(table);
        }

        /// <summary>
        /// Generate a playlist and export it to a CSV or XLSX file
        /// </summary>
        /// <param name="type"></param>
        /// <param name="timeOfDay"></param>
        /// <param name="numberOfEntries"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task ExportPlaylistAsync(PlaylistType type, TimeOfDay timeOfDay, int numberOfEntries, string filePath)
        {
            // Create a playlist
            var playlist = await _factory.PlaylistBuilder.BuildPlaylistAsync(type, timeOfDay, numberOfEntries, [], []);

            // Determine the export file type and get the appropriate exporter
            var exporter = Path.GetFileName(filePath).EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase) ?
                _factory.PlaylistXlsxExporter : _factory.PlaylistCsvExporter;

            // Export the playlist
            exporter.Export(filePath, playlist);
        }

        /// <summary>
        /// Get the content for a table cell
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetCellData(object value)
            => $"[white]{value}[/]";
    }
}