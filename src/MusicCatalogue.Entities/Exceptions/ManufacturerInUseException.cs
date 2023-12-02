using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace MusicCatalogue.Entities.Exceptions
{

    [Serializable]
    [ExcludeFromCodeCoverage]
    public class ManufacturerInUseException : Exception
    {
        public ManufacturerInUseException()
        {
        }

        public ManufacturerInUseException(string message) : base(message)
        {
        }

        public ManufacturerInUseException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ManufacturerInUseException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
