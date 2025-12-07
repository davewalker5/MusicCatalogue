using MusicCatalogue.Entities.DataExchange;
using MusicCatalogue.Entities.Interfaces;
using System.Text;

namespace MusicCatalogue.Logic.DataExchange.Equipment
{
    public class EquipmentCsvExporter : EquipmentExporterBase, IEquipmentExporter
    {
        private StreamWriter? _writer = null;

#pragma warning disable CS8618
        internal EquipmentCsvExporter(IMusicCatalogueFactory factory) : base(factory)
        {
        }
#pragma warning restore CS8618

        /// <summary>
        /// Export the register to a CSV file
        /// </summary>
        /// <param name="file"></param>
        public async Task Export(string file)
        {
            // Open the CSV file
            using (_writer = new(file, false, Encoding.UTF8))
            {
                // Iterate over the equipment register, calling the row addition methods
                await IterateOverRegister();
            }
        }

        /// <summary>
        /// Add the headers to the CSV file
        /// </summary>
        /// <param name="headers"></param>
        protected override void AddHeaders(IEnumerable<string> headers)
        {
            var csvHeaders = string.Join(",", headers);
            _writer!.WriteLine(csvHeaders);
        }

        /// <summary>
        /// Add an item of equipment to the CSV file
        /// </summary>
        /// <param name="equipment"></param>
        /// <param name="_"></param>
        protected override void AddEquipment(FlattenedEquipment equipment, int _)
        {
            _writer!.WriteLine(equipment.ToCsv());
        }
    }
}
