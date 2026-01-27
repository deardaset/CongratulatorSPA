namespace CongratulatorSPA.Server.Interfaces.Services
{
    public interface IDeletePersonService
    {
        public Task RunAsync(Guid guid);
    }
}
