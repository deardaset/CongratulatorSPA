using AutoMapper;
using CongratulatorSPA.Server.Entities;
using CongratulatorSPA.Server.Exceptions;
using CongratulatorSPA.Server.Interfaces.Repositories;
using CongratulatorSPA.Server.Interfaces.Services;
using CongratulatorSPA.Server.Models;
using Npgsql.Replication;

namespace CongratulatorSPA.Server.Services
{
    public class GetPersonService(IPersonRepository repository, IMapper mapper) : IGetPersonService
    {
        public async Task<PersonModel> RunAsync(Guid guid) => mapper.Map<PersonModel>(await repository.GetPersonByIdAsync(guid)) ?? throw new PersonNotFoundException("Person not found");
    }
}
