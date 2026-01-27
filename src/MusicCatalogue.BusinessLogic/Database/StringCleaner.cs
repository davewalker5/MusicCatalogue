using System.Globalization;

namespace MusicCatalogue.BusinessLogic.Database
{
    public static class StringCleaner
    {
        private readonly static TextInfo _textInfo = new CultureInfo("en-GB", false).TextInfo;
        private readonly static List<string> _removeLeading = new() { "A", "The" };

        /// <summary>
        /// Ensure a string is converted to a consistent case for storage in the database and
        /// subsequent searching
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string? Clean(string? s)
        {
            var clean = s;

            // Check the string isn't null or empty
            if (!string.IsNullOrEmpty(s))
            {
                // Remove invalid characters, that can cause an exception in the title case conversion, and
                // convert to lowercase to ensure that the result truly is title case. Otherwise, strings
                // such as "The BEATLES" would remain unchanged, where we really want "The Beatles".
                clean = _textInfo.ToTitleCase(RemoveInvalidCharacters(s)!.ToLower());
            }

            return clean;
        }

        /// <summary>
        /// Remove invalid characters from the string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string? RemoveInvalidCharacters(string? s)
        {
            var clean = s;

            // Check the string isn't null or empty
            if (!string.IsNullOrEmpty(s))
            {
                // Remove commas that are not permitted (foul up the CSV export) and CR/LF
                clean = s.Replace(",", "").Replace("\r", "").Replace("\n", "");
            }

            return clean;
        }

        /// <summary>
        /// Return a searchable name given an initial string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string? SearchableName(string? s)
        {
            var searchable = Clean(s);

            // Check the string isn't null or empty
            if (!string.IsNullOrEmpty(s))
            {
                // Iterate over the removable leading words
                foreach (string word in _removeLeading)
                {
                    var prefix = $"{word} ";
                    if (searchable!.StartsWith(word, StringComparison.OrdinalIgnoreCase))
                    {
                        searchable = searchable.Substring(prefix.Length);
                    }
                }
            }

            return searchable;
        }
    }
}
