using ClosedXML.Excel;
using MusicCatalogue.Entities.DataExchange;
using MusicCatalogue.Entities.Interfaces;

namespace MusicCatalogue.BusinessLogic.DataExchange.Equipment
{
    public class EquipmentXlsxExporter : EquipmentExporterBase, IEquipmentExporter
    {
        private const string WorksheetName = "Music";

        private IXLWorksheet? _worksheet = null;

#pragma warning disable CS8618
        internal EquipmentXlsxExporter(IMusicCatalogueFactory factory) : base(factory)
        {
        }
#pragma warning restore CS8618

        /// <summary>
        /// Export the register to an XLSX file
        /// </summary>
        /// <param name="file"></param>
        public async Task Export(string file)
        {
            // Create a new Excel Workbook
            using (var workbook = new XLWorkbook())
            {
                // Add a worksheet to contain the data
                _worksheet = workbook.Worksheets.Add(WorksheetName);

                // Iterate over the equipment register, calling the row addition methods. This builds the spreadsheet
                // in memory
                await IterateOverRegister();

                // Save the workbook to the specified file
                workbook.SaveAs(file);
            }
        }

        /// <summary>
        /// Add the headers to the XLSX file
        /// </summary>
        /// <param name="headers"></param>
        protected override void AddHeaders(IEnumerable<string> headers)
        {
            var columnNumber = 1;
            foreach (var header in headers)
            {
                _worksheet!.Cell(1, columnNumber).Value = header;
                columnNumber++;
            }
        }

        /// <summary>
        /// Add an item of equipment to the XLSX file
        /// </summary>
        /// <param name="equipment"></param>
        /// <param name="recordCount"></param>
        protected override void AddEquipment(FlattenedEquipment equipment, int recordCount)
        {
            var row = recordCount + 1;
            _worksheet!.Cell(row, 1).Value = equipment.Description ?? "";
            _worksheet!.Cell(row, 2).Value = equipment.Model ?? "";
            _worksheet!.Cell(row, 3).Value = equipment.SerialNumber ?? "";
            _worksheet!.Cell(row, 4).Value = equipment.EquipmentTypeName ?? "";
            _worksheet!.Cell(row, 5).Value = equipment.ManufacturerName ?? "";
            _worksheet!.Cell(row, 6).Value = (equipment.IsWishListItem ?? false).ToString();
            _worksheet!.Cell(row, 7).Value = equipment.FormattedPurchaseDate;
            _worksheet!.Cell(row, 8).Value = equipment.Price != null ? equipment.Price.ToString() : "";
            _worksheet!.Cell(row, 9).Value = equipment.RetailerName ?? "";
        }
    }
}
