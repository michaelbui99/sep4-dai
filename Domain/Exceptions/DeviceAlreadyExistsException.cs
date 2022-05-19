using System;
using System.Runtime.Serialization;

namespace Domain.Exceptions
{
    public class DeviceAlreadyExistsException : Exception
    {
        public DeviceAlreadyExistsException()
        {
        }

        protected DeviceAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public DeviceAlreadyExistsException(string? message) : base(message)
        {
        }

        public DeviceAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}