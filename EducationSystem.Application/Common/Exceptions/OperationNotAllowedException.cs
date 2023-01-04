namespace EducationSystem.Application.Common.Exceptions
{
    public class OperationNotAllowedException : Exception
    {
        public OperationNotAllowedException() : base()
        {
        }

        public OperationNotAllowedException(string message) : base(message)
        {
        }

        public OperationNotAllowedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
