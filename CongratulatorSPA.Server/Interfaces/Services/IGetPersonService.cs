using CongratulatorSPA.Server.Entities;
using CongratulatorSPA.Server.Models;

namespace CongratulatorSPA.Server.Interfaces.Services
{
    public interface IGetPersonService
    {
        public Task<PersonModel> RunAsync(Guid guid);
    }
}
