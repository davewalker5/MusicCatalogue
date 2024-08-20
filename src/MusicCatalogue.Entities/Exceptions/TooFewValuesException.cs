﻿using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class TooFewValuesException : Exception
    {
        public TooFewValuesException()
        {
        }

        public TooFewValuesException(string message) : base(message)
        {
        }

        public TooFewValuesException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}