using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Database
{
    [ExcludeFromCodeCoverage]
    public class Mood : NamedEntity
    {
        public double MorningWeight { get; set; }
        public double AfternoonWeight { get; set; }
        public double EveningWeight { get; set; }
        public double LateWeight { get; set; }
    }
}
