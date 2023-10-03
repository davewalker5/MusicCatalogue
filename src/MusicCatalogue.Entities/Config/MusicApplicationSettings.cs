using MusicCatalogue.Entities.Logging;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Config
{
    [ExcludeFromCodeCoverage]
    public class MusicApplicationSettings
    {
        public Severity MinimumLogLevel { get; set; }
        public string LogFile { get; set; } = "";
        public List<ApiEndpoint> ApiEndpoints { get; set; } = new List<ApiEndpoint>();
        public List<ApiServiceKey> ApiServiceKeys { get; set; } = new List<ApiServiceKey>();

    }
}
