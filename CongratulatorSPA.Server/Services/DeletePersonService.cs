using CongratulatorSPA.Server.Exceptions;
using CongratulatorSPA.Server.Interfaces.Repositories;
using CongratulatorSPA.Server.Interfaces.Services;

namespace CongratulatorSPA.Server.Services
{
    public class DeletePersonService(IPersonRepository repository) : IDeletePersonService
    {
        public async Task RunAsync(Guid guid)
        {
            var person = await repository.GetPersonByIdAsync(guid);
            if (person == null)
                throw new PersonNotFoundException("Person not found");

            await repository.DeletePersonAsync(person);
        }
    }
}
