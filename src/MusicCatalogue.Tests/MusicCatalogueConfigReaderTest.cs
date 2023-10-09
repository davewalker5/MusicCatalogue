using MusicCatalogue.Entities.Config;
using MusicCatalogue.Entities.Logging;
using MusicCatalogue.Logic.Config;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class MusicCatalogueConfigReaderTest
    {
        [TestMethod]
        public void ReadAppSettingsTest()
        {
            var settings = new MusicCatalogueConfigReader().Read("appsettings.json");

            Assert.AreEqual("MusicCatalogue.log", settings?.LogFile);
            Assert.AreEqual(Severity.Info, settings?.MinimumLogLevel);
            Assert.AreEqual(MusicCatalogueEnvironment.Development, settings?.Environment);

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

        [TestMethod]
        public void SeparateApiKeyFileTest()
        {
            var settings = new MusicCatalogueConfigReader().Read("separateapikeyappsettings.json");

            Assert.IsNotNull(settings?.ApiServiceKeys);
            Assert.AreEqual(1, settings?.ApiServiceKeys.Count);
            Assert.AreEqual(ApiServiceType.TheAudioDB, settings?.ApiServiceKeys.First().Service);
            Assert.AreEqual("my-separate-key", settings?.ApiServiceKeys.First().Key);
        }
    }
}
