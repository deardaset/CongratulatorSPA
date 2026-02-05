using CongratulatorSPA.Server.Entities;
using CongratulatorSPA.Server.Exceptions;
using CongratulatorSPA.Server.Interfaces.Repositories;
using CongratulatorSPA.Server.Interfaces.Services;
using CongratulatorSPA.Server.Models.Requests;
using CongratulatorSPA.Server.Models.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace CongratulatorSPA.Server.Services
{
    public class CreatePersonService(IPersonRepository repository) : ICreatePersonService
    {
        public async Task<PersonResponse> RunAsync(CreatePersonRequest request)
        {
            if (!Regex.IsMatch(request.Name, @"^[\p{L}\s]+$"))
                throw new BadRequestException("Name must be valid");
            if (request.Name.Length < 2 || request.Name.Length > 50)
                throw new BadRequestException($"Name must be longer than 2 symbols and less than 50");
            if (request.BirthDate > DateTime.Today || (DateTime.Today.Year - request.BirthDate.Year) > 110)
                throw new BadRequestException("Birthdate must be valid");

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
