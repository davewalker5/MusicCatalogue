using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Exceptions
{

    [Serializable]
    [ExcludeFromCodeCoverage]
    public class VibeInUseException : Exception
    {
        public VibeInUseException()
        {
        }

        public VibeInUseException(string message) : base(message)
        {
        }

        public VibeInUseException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
