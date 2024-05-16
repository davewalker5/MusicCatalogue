using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

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

        protected ArtistInUseException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}
