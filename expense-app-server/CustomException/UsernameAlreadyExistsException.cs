using System.Runtime.Serialization;

namespace expense_app_server.CustomException
{
    public class UsernameAlreadyExistsException : Exception
    {
        public UsernameAlreadyExistsException()
        {
        }

        public UsernameAlreadyExistsException(string? message) : base(message)
        {
        }

        public UsernameAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UsernameAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
