using MusicCatalogue.Entities.DataExchange;
using MusicCatalogue.Entities.Interfaces;

namespace MusicCatalogue.BusinessLogic.DataExchange.Equipment
{
    public abstract class EquipmentExporterBase : DataExchangeBase
    {
        private readonly string[] ColumnHeaders =
        {
            "Description",
            "Model",
            "Serial Number",
            "Type",
            "Manufacturer",
            "Wish List",
            "Purchase Date",
            "Price",
            "Retailer"
        };

        public event EventHandler<EquipmentDataExchangeEventArgs>? EquipmentExport;

        protected EquipmentExporterBase(IMusicCatalogueFactory factory) : base(factory)
        {
        }

        /// <summary>
        /// Method to add headers to the output
        /// </summary>
        /// <param name="headers"></param>
        protected abstract void AddHeaders(IEnumerable<string> headers);

        /// <summary>
        /// Method to add a new flattened equipment record to the output
        /// </summary>
        /// <param name="equipment"></param>
        /// <param name="recordNumber"></param>
        protected abstract void AddEquipment(FlattenedEquipment equipment, int recordNumber);

        /// <summary>
        /// Iterate over the equipment register calling the methods supplied by the child class to add
        /// headers and to add each track to the output
        /// </summary>
        protected async Task IterateOverRegister()
        {
            // Call the method, supplied by the child class, to add the headers to the output
            AddHeaders(ColumnHeaders);

            // Initialise the record count
            int count = 0;

            // Retrieve a list of equipment records then iterate over them
            var equipment = await _factory.Equipment.ListAsync(x => true);
            foreach (var item in equipment)
            {
                // Construct a flattened record for this item of equipment
                var flattened = new FlattenedEquipment
                {
                    Description = item.Description,
                    Model = item.Model,
                    SerialNumber = item.SerialNumber,
                    EquipmentTypeName = item.EquipmentType!.Name,
                    ManufacturerName = item.Manufacturer!.Name,
                    IsWishListItem = item.IsWishListItem,
                    Purchased = item.Purchased,
                    Price = item.Price,
                    RetailerName = item.Retailer?.Name
                };

                // Call the method to add this item of equipment to the file
                count++;
                AddEquipment(flattened, count);

                // Raise the equipment exported event
                EquipmentExport?.Invoke(this, new EquipmentDataExchangeEventArgs { RecordCount = count, Equipment = flattened });
            }
        }
    }
}
