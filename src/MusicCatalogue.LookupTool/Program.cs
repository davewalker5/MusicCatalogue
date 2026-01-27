using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Data;
using MusicCatalogue.Entities.CommandLine;
using MusicCatalogue.Entities.Config;
using MusicCatalogue.Entities.Logging;
using MusicCatalogue.BusinessLogic.CommandLine;
using MusicCatalogue.BusinessLogic.Config;
using MusicCatalogue.BusinessLogic.Factory;
using MusicCatalogue.BusinessLogic.Logging;
using MusicCatalogue.LookupTool.Entities;
using MusicCatalogue.LookupTool.Interfaces;
using MusicCatalogue.LookupTool.Logic;
using System.Diagnostics;
using System.Reflection;

namespace MusicCatalogue.LookupTool
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

            try
            {
                // Parse the command line
                CommandLineParser parser = new(new HelpTabulator());
                parser.Add(CommandLineOptionType.Help, true, "--help", "-h", "Show command line help", 0, 0);
                parser.Add(CommandLineOptionType.Lookup, true, "--lookup", "-l", "Lookup an album and display its details", 3, 3);
                parser.Add(CommandLineOptionType.Import, true, "--import", "-i", "Import data from a CSV format file", 1, 1);
                parser.Add(CommandLineOptionType.Export, true, "--export", "-e", "Export the collection or equipment register to a CSV file or Excel Workbook", 2, 2);
                parser.Add(CommandLineOptionType.Update, true, "--update", "-u", "Apply the latest database migrations", 0, 0);
                parser.Parse(args);

                // If help's been requested, show help and exit
                if (parser.IsPresent(CommandLineOptionType.Help))
                {
                    parser.Help();
                }
                else
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

                    // Configure the business logic factory
                    var context = new MusicCatalogueDbContextFactory().CreateDbContext([]);
                    var factory = new MusicCatalogueFactory(context);

                    // If this is an update, apply the latest migrations
                    if (parser.IsPresent(CommandLineOptionType.Update))
                    {
                        context.Database.Migrate();
                        Console.WriteLine($"Applied the latest database migrations");
                    }

                    // If this is a lookup, look up the album details
                    if (parser.IsPresent(CommandLineOptionType.Lookup))
                    {
                        // Determine the target for new albums (catalogue or wish list) and lookup the album
                        var values = parser.GetValues(CommandLineOptionType.Lookup);
                        var targetType = (TargetType)Enum.Parse(typeof(TargetType), values![2]);
                        var storeInWishList = targetType == TargetType.wishlist;
                        await new AlbumLookup(logger, factory, settings!).LookupAlbum(values[0], values[1], storeInWishList);
                    }

                    // If this is an import, import data from the specified CSV file
                    if (parser.IsPresent(CommandLineOptionType.Import))
                    {
                        var values = parser.GetValues(CommandLineOptionType.Import);
                        new DataImport(logger, factory).Import(values![0]);
                    }

                    // If this is an export, export the collection to the specified file
                    if (parser.IsPresent(CommandLineOptionType.Export))
                    {
                        var values = parser.GetValues(CommandLineOptionType.Export);
                        var exportType = (ExportType)Enum.Parse(typeof(ExportType), values![0]);
                        IDataExporter exporter = exportType == ExportType.music ? new CatalogueExporter(logger, factory) : new EquipmentExporter(logger, factory);
                        exporter.Export(values[1]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}