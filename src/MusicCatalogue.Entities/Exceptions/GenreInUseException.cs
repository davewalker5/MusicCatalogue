using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

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

        protected GenreInUseException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}