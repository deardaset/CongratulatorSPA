using CongratulatorSPA.Server.Entities;

namespace CongratulatorSPA.Server.Interfaces.Services
{
    public interface IGetPersonService
    {
        public Task<Person> RunAsync(Guid guid);
    }
}
