using MusicCatalogue.BusinessLogic.Database;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class StringCleanerTest
    {
        [TestMethod]
        public void RemoveInvalidCharactersTest()
        {
            var result = StringCleaner.RemoveInvalidCharacters(",\r\n");
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void CleanTest()
        {
            var result = StringCleaner.Clean("tHe, BEATles\r\n");
            Assert.AreEqual("The Beatles", result);
        }

        [TestMethod]
        public void SearchableNameTest()
        {
            var result = StringCleaner.SearchableName("tHe, BEATles\r\n");
            Assert.AreEqual("Beatles", result);
        }
    }
}
