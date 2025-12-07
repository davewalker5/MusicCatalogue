using System.Diagnostics.CodeAnalysis;

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
    }
}