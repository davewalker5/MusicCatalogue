using MusicCatalogue.Entities.Logging;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Config
{
    [ExcludeFromCodeCoverage]
    public class MusicApplicationSettings
    {
        public string Secret { get; set; } = "";
        public int TokenLifespanMinutes { get; set; }
        public Severity MinimumLogLevel { get; set; }
        public string LogFile { get; set; } = "";
        public MusicCatalogueEnvironment Environment { get; set; }
        public string CatalogueExportPath { get; set; } = "";
        public string ReportsExportPath { get; set; } = "";
        public string SessionExportPath { get; set; } = "";
        public List<Secret> Secrets { get; set; } = new List<Secret>();
        public List<ApiEndpoint> ApiEndpoints { get; set; } = new List<ApiEndpoint>();
        public List<ApiServiceKey> ApiServiceKeys { get; set; } = new List<ApiServiceKey>();

    }
}
