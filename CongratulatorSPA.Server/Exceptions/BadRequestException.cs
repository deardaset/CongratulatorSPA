namespace CongratulatorSPA.Server.Exceptions
{
    public class BadRequestException : CongratulatorException
    {
        public BadRequestException(string message) : base(message)
        {
            StatusCode = 400;
        }
    }
}
