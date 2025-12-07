using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class ArtistInUseException : Exception
    {
        public ArtistInUseException()
        {
        }

        public ArtistInUseException(string message) : base(message)
        {
        }

        public ArtistInUseException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
