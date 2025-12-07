using System.Diagnostics.CodeAnalysis;

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
    }
}
