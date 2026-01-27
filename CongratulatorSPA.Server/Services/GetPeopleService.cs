using CongratulatorSPA.Server.Entities;
using CongratulatorSPA.Server.Interfaces.Repositories;
using CongratulatorSPA.Server.Interfaces.Services;

namespace CongratulatorSPA.Server.Services
{
    public class GetPeopleService(IPersonRepository repository) : IGetPeopleService
    {
        public async Task<List<Person>> RunAsync()
        {
            var people = await repository.GetAllPeopleAsync();
            return people;
        }
    }
}
