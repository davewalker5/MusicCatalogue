using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.CommandLine
{
    [ExcludeFromCodeCoverage]
    public class CommandLineOptionValue
    {
        public CommandLineOption? Option { get; set; }
        public List<string> Values { get; private set; } = new List<string>();
    }
}
