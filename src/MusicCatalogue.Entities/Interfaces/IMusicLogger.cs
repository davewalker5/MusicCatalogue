using MusicCatalogue.Entities.Logging;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IMusicLogger
    {
        void Initialise(string logFile, Severity minimumSeverityToLog);
        void LogMessage(Severity severity, string message);
        void LogException(Exception ex);
    }
}
