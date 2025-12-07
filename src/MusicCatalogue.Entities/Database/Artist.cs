using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Database
{
    [ExcludeFromCodeCoverage]
    public class Artist : NamedEntity
    {
        public string? SearchableName { get; set; } = null;

        public ICollection<Album>? Albums { get; set; }
    }
}
