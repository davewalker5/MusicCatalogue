using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace MusicCatalogue.Entities.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class EquipmentTypeInUseException : Exception
    {
        public EquipmentTypeInUseException()
        {
        }

        public EquipmentTypeInUseException(string message) : base(message)
        {
        }

        public EquipmentTypeInUseException(string message, Exception inner) : base(message, inner)
        {
        }

        protected EquipmentTypeInUseException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
