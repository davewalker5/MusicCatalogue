using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class GenreInUseException : Exception
    {
        public GenreInUseException()
        {
        }

        public GenreInUseException(string message) : base(message)
        {
        }

        public GenreInUseException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}