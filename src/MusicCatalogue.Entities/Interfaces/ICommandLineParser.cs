using MusicCatalogue.Entities.CommandLine;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface ICommandLineParser
    {
        void Add(CommandLineOptionType optionType, bool isOperation, string name, string shortName, string description, int minimumNumberOfValues, int maximumNumberOfValues);
        List<string>? GetValues(CommandLineOptionType optionType);
        bool IsPresent(CommandLineOptionType optionType);
        void Help();
        void Parse(IEnumerable<string> args);
    }
}