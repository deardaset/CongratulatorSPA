using CongratulatorSPA.Server.Exceptions;
using CongratulatorSPA.Server.Interfaces.Repositories;
using CongratulatorSPA.Server.Interfaces.Services;
using CongratulatorSPA.Server.Models.Requests;
using CongratulatorSPA.Server.Models.Responses;
using System.Text.RegularExpressions;

namespace CongratulatorSPA.Server.Services
{
    public class UpdatePersonService(IPersonRepository repository) : IUpdatePersonService
    {
        public async Task<PersonResponse> RunAsync(Guid guid, UpdatePersonRequest request)
        {
            if (!Regex.IsMatch(request.Name, @"^[\p{L}\s]+$"))
                throw new BadRequestException("Name must be valid");
            if (request.Name.Length < 2 || request.Name.Length > 50)
                throw new BadRequestException($"Name must be longer than 2 symbols and less than 50");
            if (request.BirthDate > DateTime.Today || (DateTime.Today.Year - request.BirthDate.Year) > 110)
                throw new BadRequestException("Birthdate must be valid");

            var person = await repository.GetPersonByIdAsync(guid);
            if (person == null)
                throw new NotFoundException("Person not found");

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
