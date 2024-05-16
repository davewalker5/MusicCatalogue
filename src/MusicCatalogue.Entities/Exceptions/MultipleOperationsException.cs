using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace MusicCatalogue.Entities.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class MultipleOperationsException : Exception
    {
        public MultipleOperationsException()
        {
        }

        public MultipleOperationsException(string message) : base(message)
        {
        }

        public MultipleOperationsException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MultipleOperationsException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}

