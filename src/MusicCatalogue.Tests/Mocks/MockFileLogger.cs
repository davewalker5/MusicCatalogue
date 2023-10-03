﻿using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Logging;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    public class MockFileLogger : IMusicLogger
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
