using System;
using System.Runtime.Serialization;

namespace Keeker.Core.Exceptions
{
    [Serializable]
    public class BadHttpDataException : Exception
    {
        public BadHttpDataException()
            : base("Bad HTTP data")
        {
        }

        public BadHttpDataException(string message)
            : base(message)
        {
        }

        public BadHttpDataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected BadHttpDataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}