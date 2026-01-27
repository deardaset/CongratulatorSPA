using CongratulatorSPA.Server.Entities;

namespace CongratulatorSPA.Server.Interfaces.Services
{
    public interface IGetPeopleService
    {
        public Task<List<Person>> RunAsync();
    }
}
