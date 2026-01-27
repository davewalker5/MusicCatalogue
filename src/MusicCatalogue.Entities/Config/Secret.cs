using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Config
{
    [ExcludeFromCodeCoverage]
    public class Secret
    {
        public string Name { get; set; } = "";
        public string Value { get; set; } = "";
    }
}
