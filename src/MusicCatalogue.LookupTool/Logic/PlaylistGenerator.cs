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
            // Generate the playlist
            var playlist = await _factory.ArtistPlaylistBuilder.BuildPlaylist(type, timeOfDay, numberOfEntries);
            var albums = await _factory.ArtistPlaylistBuilder.PickPlaylistAlbums(playlist);

            // Create a table and add the columns to it
            var table = new Table();
            table.AddColumn("#");
            table.AddColumn("Artist");
            table.AddColumn("Album");

            // Add the albums to the table
            for (int i = 0; i < albums.Count; i++)
            {
                var rowData = new string[] {
                    GetCellData(i + 1),
                    GetCellData(albums[i].Artist!.Name),
                    GetCellData(albums[i].Title)
                };

                table.AddRow(rowData);
            }

            AnsiConsole.Write(table);
        }

        private string GetCellData(object value)
            => $"[white]{value}[/]";
    }
}