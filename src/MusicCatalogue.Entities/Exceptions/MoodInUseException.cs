using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Exceptions
{

    [Serializable]
    [ExcludeFromCodeCoverage]
    public class MoodInUseException : Exception
    {
        public MoodInUseException()
        {
        }

        public MoodInUseException(string message) : base(message)
        {
        }

        public MoodInUseException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
