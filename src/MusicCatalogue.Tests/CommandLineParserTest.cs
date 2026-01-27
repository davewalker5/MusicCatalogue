using MusicCatalogue.Entities.CommandLine;
using MusicCatalogue.Entities.Exceptions;
using MusicCatalogue.BusinessLogic.CommandLine;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class CommandLineParserTest
    {
        private CommandLineParser? _parser;

        [TestInitialize]
        public void TestInitialise()
        {
            _parser = new CommandLineParser();
            _parser.Add(CommandLineOptionType.Lookup, true, "--lookup", "-l", "Lookup an album and display its details", 2, 2);
            _parser.Add(CommandLineOptionType.Import, true, "--import", "-i", "Import data from a CSV format file", 1, 1);
        }

        [TestMethod]
        public void ValidUsingNamesTest()
        {
            string[] args = new string[]{ "--lookup", "The Beatles", "Let It Be" };
            _parser!.Parse(args);

            var values = _parser?.GetValues(CommandLineOptionType.Lookup);
            Assert.IsNotNull(values);
            Assert.AreEqual(2, values.Count);
            Assert.AreEqual("The Beatles", values[0]);
            Assert.AreEqual("Let It Be", values[1]);
        }

        [TestMethod]
        public void ValidUsingShortNamesTest()
        {
            string[] args = new string[] { "-l", "The Beatles", "Let It Be" };
            _parser!.Parse(args);

            var values = _parser?.GetValues(CommandLineOptionType.Lookup);
            Assert.IsNotNull(values);
            Assert.AreEqual(2, values.Count);
            Assert.AreEqual("The Beatles", values[0]);
            Assert.AreEqual("Let It Be", values[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(TooFewValuesException))]
        public void TooFewArgumentsFailsTest()
        {
            string[] args = new string[] { "-l", "The Beatles" };
            _parser!.Parse(args);
        }

        [TestMethod]
        [ExpectedException(typeof(TooManyValuesException))]
        public void TooManyArgumentsFailsTest()
        {
            string[] args = new string[] { "-l", "The Beatles", "Let It Be", "Extra Argument" };
            _parser!.Parse(args);
        }

        [TestMethod]
        [ExpectedException(typeof(UnrecognisedCommandLineOptionException))]
        public void UnrecognisedOptionNameFailsTest()
        {
            string[] args = new string[] { "--oops", "The Beatles", "Let It Be" };
            _parser!.Parse(args);
        }

        [TestMethod]
        [ExpectedException(typeof(UnrecognisedCommandLineOptionException))]
        public void UnrecognisedOptionShortNameFailsTest()
        {
            string[] args = new string[] { "-o", "The Beatles", "Let It Be" };
            _parser!.Parse(args);
        }

        [TestMethod]
        [ExpectedException(typeof(MalformedCommandLineException))]
        public void MalformedCommandLineFailsTest()
        {
            string[] args = new string[] { "The Beatles", "--lookup", "Let It Be" };
            _parser!.Parse(args);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateOptionException))]
        public void DuplicateOptionTypeFailsTest()
        {
            _parser!.Add(CommandLineOptionType.Lookup, true, "--other-lookup", "-ol", "Duplicate option type", 2, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateOptionException))]
        public void DuplicateOptionNameFailsTest()
        {
            _parser!.Add(CommandLineOptionType.Unknown, true, "--lookup", "-ol", "Duplicate option name", 2, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateOptionException))]
        public void DuplicateOptionShortNameFailsTest()
        {
            _parser!.Add(CommandLineOptionType.Unknown, true, "--other-lookup", "-l", "Duplicate option shortname", 2, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(MultipleOperationsException))]
        public void MultipleOperationsFailsTest()
        {
            string[] args = new string[] { "--lookup", "The Beatles", "Let It Be", "--import", "a_file.csv" };
            _parser!.Parse(args);
        }
    }
}
