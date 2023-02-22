using System.Runtime.Serialization;

namespace expense_app_server.CustomException
{
    [Serializable]
    internal class PasswordException : Exception
    {
        public PasswordException()
        {
        }

        public PasswordException(string? message) : base(message)
        {
        }

        public PasswordException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PasswordException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}