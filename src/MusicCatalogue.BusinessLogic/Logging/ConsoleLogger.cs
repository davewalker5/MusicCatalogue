using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Logging;
using System.Diagnostics;

namespace MusicCatalogue.BusinessLogic.Logging
{
    public class ConsoleLogger : IMusicLogger
    {
        public void Initialise(string logFile, Severity minimumSeverityToLog)
        {
        }

        public void LogMessage(Severity severity, string message)
        {
            Debug.Print($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} [{severity.ToString()}] {message}");
        }

        public void LogException(Exception ex)
        {
            LogMessage(Severity.Error, ex.Message);
            LogMessage(Severity.Error, ex.ToString());
        }
    }
}
