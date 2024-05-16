using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace MusicCatalogue.Entities.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class UnrecognisedCommandLineOptionException : Exception
    {
        public UnrecognisedCommandLineOptionException()
        {
        }

        public UnrecognisedCommandLineOptionException(string message) : base(message)
        {
        }

        public UnrecognisedCommandLineOptionException(string message, Exception inner) : base(message, inner)
        {
        }

        protected UnrecognisedCommandLineOptionException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}
