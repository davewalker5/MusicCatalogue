using MusicCatalogue.Entities.DataExchange;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Logging;

namespace MusicCatalogue.LookupTool.Logic
{
    internal class EquipmentExporter : DataExportBase
    {
        public EquipmentExporter(IMusicLogger logger, IMusicCatalogueFactory factory) : base(logger, factory)
        {
        }

        /// <summary>
        /// Export the equipment register to the specified file
        /// </summary>
        /// <param name="file"></param>
        public override void Export(string file)
        {
            Console.WriteLine($"Exporting the equipment register to {file} ...");

            // Use the file extension to decide which exporter to use
            var extension = Path.GetExtension(file).ToLower();
            IEquipmentExporter? exporter = extension == ".xlsx" ? Factory.EquipmentXlsxExporter : Factory.EquipmentCsvExporter;

            try
            {
                // Register a handler for the "track imported" event and import the file 
                exporter!.EquipmentExport += OnEquipmentExported;
                Task.Run(() => exporter.Export(file)).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Export error: {ex.Message}");
                Logger.LogMessage(Severity.Info, $"Export error: {ex.Message}");
                Logger.LogException(ex);
            }
            finally
            {
                // Un-register the event handler
                exporter!.EquipmentExport -= OnEquipmentExported;
            }
        }

        /// <summary>
        /// Handler called when an item of equipment is imported
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnEquipmentExported(object? sender, EquipmentDataExchangeEventArgs e)
        {
            if (e.Equipment != null)
            {
                Console.WriteLine($"Exported {e.Equipment.EquipmentTypeName} : {e.Equipment.Description}");
            }
        }
    }
}
