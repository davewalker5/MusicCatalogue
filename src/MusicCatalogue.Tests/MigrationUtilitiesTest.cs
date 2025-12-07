using MusicCatalogue.Data;

namespace MusicCatalogue.Tests
{
    [TestClass]
    public class MigrationUtilitiesTest
    {
        [TestMethod]
        public void GetNamespaceTest()
        {
            var dataAssemblyNamespace = MigrationUtilities.GetSqlScriptNamespace();
            Assert.AreEqual("MusicCatalogue.Data.Sql", dataAssemblyNamespace);
        }

        [TestMethod]
        public void ReadMigrationScriptTest()
        {
            var scriptContent = MigrationUtilities.ReadMigrationSqlScript("GenreDataMigration.sql");
            Assert.IsFalse(string.IsNullOrEmpty(scriptContent));
        }
    }
}
