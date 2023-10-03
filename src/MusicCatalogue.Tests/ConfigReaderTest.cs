﻿using MusicCatalogue.Entities.Config;
using MusicCatalogue.Entities.Logging;
using MusicCatalogue.Logic.Config;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class ConfigReaderTest
    {
        [TestMethod]
        public void ReadAppSettingsTest()
        {
            var settings = new ConfigReader<MusicApplicationSettings>().Read("appsettings.json");

            Assert.AreEqual("MusicCatalogue.log", settings?.LogFile);
            Assert.AreEqual(Severity.Info, settings?.MinimumLogLevel);

            Assert.IsNotNull(settings?.ApiEndpoints);
            Assert.AreEqual(1, settings?.ApiEndpoints.Count);
            Assert.AreEqual(ApiEndpointType.Albums, settings?.ApiEndpoints.First().EndpointType);
            Assert.AreEqual(ApiServiceType.TheAudioDB, settings?.ApiEndpoints.First().Service);
            Assert.AreEqual("https://theaudiodb.p.rapidapi.com/searchalbum.php", settings?.ApiEndpoints.First().Url);

            Assert.IsNotNull(settings?.ApiServiceKeys);
            Assert.AreEqual(1, settings?.ApiServiceKeys.Count);
            Assert.AreEqual(ApiServiceType.TheAudioDB, settings?.ApiServiceKeys.First().Service);
            Assert.AreEqual("my-key", settings?.ApiServiceKeys.First().Key);
        }
    }
}
