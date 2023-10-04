using System.Globalization;

namespace MusicCatalogue.Logic.Database
{
    public static class StringCleaner
    {
        private readonly static TextInfo _textInfo = new CultureInfo("en-GB", false).TextInfo;

        /// <summary>
        /// Ensure a string is converted to a consistent case for storage in the database and
        /// subsequent searching
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Clean(string s)
        {
            // Conversion to lowercase, first, ensures that the result truly is title case. Otherwise,
            // strings such as "The BEATLES" would remain unchanged, where we really want "The Beatles"
            var clean = _textInfo.ToTitleCase(s.ToLower());
            return clean;
        }
    }
}
