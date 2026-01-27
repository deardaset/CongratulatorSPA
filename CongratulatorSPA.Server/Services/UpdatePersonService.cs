using CongratulatorSPA.Server.Exceptions;
using CongratulatorSPA.Server.Interfaces.Repositories;
using CongratulatorSPA.Server.Interfaces.Services;
using CongratulatorSPA.Server.Models.Requests;
using CongratulatorSPA.Server.Models.Responses;

namespace CongratulatorSPA.Server.Services
{
    public class UpdatePersonService(IPersonRepository repository) : IUpdatePersonService
    {
        public async Task<PersonResponse> RunAsync(Guid guid, UpdatePersonRequest request)
        {
            var person = await repository.GetPersonByIdAsync(guid);
            if (person == null)
                throw new PersonNotFoundException("Person not found");

            bool hasChanges = false;

            if (request.Name is { } name && name != person.Name)
                (person.Name, hasChanges) = (name, true);
            if (request.BirthDate is { } date && date != person.BirthDate)
                (person.BirthDate, hasChanges) = (date, true);
            if (request.RelationshipType is { } type && type != person.RelationshipType)
                (person.RelationshipType, hasChanges) = (type, true);
            
            if (hasChanges)
            {
                await repository.UpdatePersonAsync(person);
            }

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
