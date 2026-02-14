using CongratulatorSPA.Server.Entities;
using CongratulatorSPA.Server.Interfaces.Repositories;
using CongratulatorSPA.Server.Interfaces.Services;
using CongratulatorSPA.Server.Models;
using Npgsql.Replication;

namespace CongratulatorSPA.Server.Services
{
    public class GetPersonService(IPersonRepository repository) : IGetPersonService
    {
        public async Task<PersonModel> RunAsync(Guid guid)
        {
            return await repository.GetPersonByIdAsync(guid);
        }
    }
}
