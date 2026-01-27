using CongratulatorSPA.Server.Entities;
using CongratulatorSPA.Server.Interfaces.Repositories;
using CongratulatorSPA.Server.Interfaces.Services;
using CongratulatorSPA.Server.Models.Requests;
using CongratulatorSPA.Server.Models.Responses;

namespace CongratulatorSPA.Server.Services
{
    public class CreatePersonService(IPersonRepository repository) : ICreatePersonService
    {
        public async Task<PersonResponse> RunAsync(CreatePersonRequest request)
        {
            var person = new Person
            {
                Name = request.Name,
                BirthDate = request.BirthDate,
                RelationshipType = request.RelationshipType
            };
            await repository.CreatePersonAsync(person);
            return new PersonResponse
            {
                Guid = person.Guid,
                Name = person.Name,
                BirthDate = person.BirthDate,
                RelationshipType = person.RelationshipType
            };
        }
    }
}
