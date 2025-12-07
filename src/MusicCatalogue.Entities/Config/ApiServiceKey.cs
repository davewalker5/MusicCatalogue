using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Config
{
    [ExcludeFromCodeCoverage]
    public class ApiServiceKey
    {
        public ApiServiceType Service { get; set; }
        public string Key { get; set; } = "";
    }
}
