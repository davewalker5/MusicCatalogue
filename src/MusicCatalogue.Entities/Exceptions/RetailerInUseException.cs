using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace MusicCatalogue.Entities.Exceptions
{

    [Serializable]
    [ExcludeFromCodeCoverage]
    public class RetailerInUseException : Exception
    {
        public RetailerInUseException()
        {
        }

        public RetailerInUseException(string message) : base(message)
        {
        }

        public RetailerInUseException(string message, Exception inner) : base(message, inner)
        {
        }

        protected RetailerInUseException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}