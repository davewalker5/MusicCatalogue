using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Api.Entities
{
    [ExcludeFromCodeCoverage]
    public class EquipmentExportWorkItem : BackgroundWorkItem
    {
        public string FileName { get; set; } = "";

        public override string ToString()
        {
            return $"{base.ToString()}, FileName = {FileName}";
        }
    }
}
