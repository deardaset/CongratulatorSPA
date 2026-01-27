using CongratulatorSPA.Server.Entities;
using CongratulatorSPA.Server.Interfaces.Repositories;
using CongratulatorSPA.Server.Interfaces.Services;
using Npgsql.Replication;

namespace CongratulatorSPA.Server.Services
{
    public class GetPersonService(IPersonRepository repository) : IGetPersonService
    {
        public async Task<Person> RunAsync(Guid guid)
        {
            var person = await repository.GetPersonByIdAsync(guid);
            return person;
        }
    }
}
