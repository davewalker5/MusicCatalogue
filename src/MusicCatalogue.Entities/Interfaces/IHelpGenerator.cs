using MusicCatalogue.Entities.CommandLine;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IHelpGenerator
    {
        void Generate(IEnumerable<CommandLineOption> options);
    }
}
