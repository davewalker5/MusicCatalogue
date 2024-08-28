using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class DuplicateGenreException : Exception
    {
        public DuplicateGenreException()
        {
        }

        public DuplicateGenreException(string message) : base(message)
        {
        }

        public DuplicateGenreException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}