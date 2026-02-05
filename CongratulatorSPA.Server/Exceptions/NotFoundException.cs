namespace CongratulatorSPA.Server.Exceptions
{
    public class NotFoundException : CongratulatorException
    {
        public NotFoundException(string message) : base(message)
        {
            StatusCode = 404;
        }
    }
}
