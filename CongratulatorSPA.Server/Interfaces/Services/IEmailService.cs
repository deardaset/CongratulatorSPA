namespace CongratulatorSPA.Server.Interfaces.Services
{
    public interface IEmailService
    {
        public Task SendBirthdayAsync(string email, string name, CancellationToken cancellationToken);
    }
}
