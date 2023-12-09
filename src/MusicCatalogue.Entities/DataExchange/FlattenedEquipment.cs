using MusicCatalogue.Entities.Exceptions;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace MusicCatalogue.Entities.DataExchange
{
    [ExcludeFromCodeCoverage]
    public class FlattenedEquipment
    {
        protected const string DateTimeFormat = "dd/MM/yyyy";

        public const int DescriptionField = 0;
        public const int ModelField = 1;
        private const int SerialNumberField = 2;
        private const int EquipmentTypeField = 3;
        private const int ManufacturerField = 4;
        private const int WishlistItemField = 5;
        private const int PurchasedField = 6;
        private const int PriceField = 7;
        private const int RetailerField = 8;
        private const int NumberOfFields = 9;

        public string Description { get; set; } = "";
        public string? Model { get; set; }
        public string? SerialNumber { get; set; }
        public string EquipmentTypeName { get; set; } = "";
        public string ManufacturerName { get; set; } = "";
        public bool? IsWishListItem { get; set; }
        public DateTime? Purchased { get; set; }
        public decimal? Price { get; set; }
        public string? RetailerName { get; set; }

        /// <summary>
        /// Purchase date formatted per the DateTimeFormat
        /// </summary>
        public string FormattedPurchaseDate
        {
            get
            {
                return Purchased != null ? (Purchased ?? DateTime.Now).ToString(DateTimeFormat) : "";
            }
        }


        /// <summary>
        /// Create a representation of the flattened equipment record in CSV format
        /// </summary>
        /// <returns></returns>
        public string ToCsv()
        {
            var wishListString = (IsWishListItem ?? false).ToString();
            var purchasedDateString = Purchased != null ? (Purchased ?? DateTime.Now).ToString(DateTimeFormat) : "";
            var priceString = Price != null ? Price.ToString() : "";

            StringBuilder builder = new StringBuilder();
            AppendField(builder, Description);
            AppendField(builder, Model ?? "");
            AppendField(builder, SerialNumber ?? "");
            AppendField(builder, EquipmentTypeName);
            AppendField(builder, ManufacturerName);
            AppendField(builder, wishListString);
            AppendField(builder, purchasedDateString);
            AppendField(builder, priceString);
            AppendField(builder, RetailerName ?? "");

            return builder.ToString();
        }

        /// <summary>
        /// Create a flattened equipment record from a CSV string
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static FlattenedEquipment FromCsv(IList<string> fields)
        {
            // Check we have the required number of fields
            if ((fields == null) || (fields.Count != NumberOfFields))
            {
                throw new InvalidRecordFormatException("Incorrect number of CSV fields");
            }

            // Get the model and serial number, both of which may be NULL
            string? model = !string.IsNullOrEmpty(fields[ModelField]) ? fields[ModelField] : null;
            string? serialNumber = !string.IsNullOrEmpty(fields[SerialNumberField]) ? fields[SerialNumberField] : null;

            // Determine the purchase date
            DateTime? purchasedDate = null;
            if (!string.IsNullOrEmpty(fields[PurchasedField]))
            {
                purchasedDate = DateTime.ParseExact(fields[PurchasedField], DateTimeFormat, null);
            }

            // Determine the price
            decimal? price = !string.IsNullOrEmpty(fields[PriceField]) ? decimal.Parse(fields[PriceField]) : null;

            // Create a new "flattened" record containing equipment details
            return new FlattenedEquipment
            {
                Description = fields[DescriptionField],
                Model = fields[ModelField],
                SerialNumber = fields[SerialNumberField],
                EquipmentTypeName = fields[EquipmentTypeField],
                ManufacturerName = fields[ManufacturerField],
                IsWishListItem = bool.Parse(fields[WishlistItemField]),
                Purchased = purchasedDate,
                Price = price,
                RetailerName = fields[RetailerField]
            };
        }

        /// <summary>
        /// Append a value to a string builder holding a representation of a flattened equpiment record in CSV format
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="value"></param>
        private static void AppendField(StringBuilder builder, object? value)
        {
            // Add a separator if there are already fields in the line under construction
            if (builder.Length > 0)
            {
                builder.Append(',');
            }

            // Convert the value to string and see if it contains the delimiter
            var stringValue = (value?.ToString() ?? "").Replace('"', '\'');
            var containsDelimiter = !string.IsNullOrEmpty(stringValue) && stringValue.Contains(',');

            // Add the value to the builder, quoting it if needed
            if (containsDelimiter) builder.Append('"');
            builder.Append(stringValue);
            if (containsDelimiter) builder.Append('"');
        }
    }
}
