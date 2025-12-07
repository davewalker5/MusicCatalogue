using System.Diagnostics.CodeAnalysis;

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
    }
}
