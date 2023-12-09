using MusicCatalogue.Data;
using MusicCatalogue.Entities.CommandLine;
using MusicCatalogue.Entities.Config;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Logging;
using MusicCatalogue.Logic.CommandLine;
using MusicCatalogue.Logic.Config;
using MusicCatalogue.Logic.Factory;
using MusicCatalogue.Logic.Logging;
using MusicCatalogue.LookupTool.Entities;
using MusicCatalogue.LookupTool.Interfaces;
using MusicCatalogue.LookupTool.Logic;
using System.Diagnostics;
using System.Reflection;

namespace MusicCatalogue.LookupPoC
{
    public static class Program
    {
        /// <summary>
        /// Application entry point
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task Main(string[] args)
        {
            // Read the application settings
            MusicApplicationSettings? settings = new MusicCatalogueConfigReader().Read("appsettings.json");

            // Configure the log file
            FileLogger logger = new FileLogger();
            logger.Initialise(settings!.LogFile, settings.MinimumLogLevel);

            // Get the version number and application title
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo info = FileVersionInfo.GetVersionInfo(assembly.Location);
            var title = $"Music Catalogue Lookup Tool v{info.FileVersion}";

            // Log the startup messages
            Console.WriteLine($"{title}\n");
            logger.LogMessage(Severity.Info, new string('=', 80));
            logger.LogMessage(Severity.Info, title);

            try
            {
                // Parse the command line
                CommandLineParser parser = new();
                parser.Add(CommandLineOptionType.Lookup, true, "--lookup", "-l", "Lookup an album and display its details", 3, 3);
                parser.Add(CommandLineOptionType.Import, true, "--import", "-i", "Import data from a CSV format file", 1, 1);
                parser.Add(CommandLineOptionType.Export, true, "--export", "-e", "Export the collection or equipment register to a CSV file or Excel Workbook", 2, 2);
                parser.Parse(args);

                // Configure the business logic factory
                var context = new MusicCatalogueDbContextFactory().CreateDbContext(Array.Empty<string>());
                var factory = new MusicCatalogueFactory(context);

                // If this is a lookup, look up the album details
                var values = parser.GetValues(CommandLineOptionType.Lookup);
                if (values != null)
                {
                    // Determine the target for new albums (catalogue or wish list) and lookup the album
                    var targetType = (TargetType)Enum.Parse(typeof(TargetType), values[2]);
                    var storeInWishList = targetType == TargetType.wishlist;
                    await new AlbumLookup(logger, factory, settings!).LookupAlbum(values[0], values[1], storeInWishList);
                }

                // If this is an import, import data from the specified CSV file
                values = parser.GetValues(CommandLineOptionType.Import);
                if (values != null)
                {
                    new DataImport(logger, factory).Import(values[0]);
                }

                // If this is an export, export the collection to the specified file
                values = parser.GetValues(CommandLineOptionType.Export);
                if (values != null)
                {
                    var exportType = (ExportType)Enum.Parse(typeof(ExportType), values[0]);
                    IDataExporter exporter = exportType == ExportType.music ? new CatalogueExporter(logger, factory) : new EquipmentExporter(logger, factory);
                    exporter.Export(values[1]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}