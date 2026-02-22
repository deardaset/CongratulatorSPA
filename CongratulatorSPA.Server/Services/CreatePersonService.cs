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
    public class CreatePersonService(IPersonRepository repository, IStorageService storage) : ICreatePersonService
    {
        public async Task<PersonResponse> RunAsync(CreatePersonRequest request)
        {
            string? photoUrl = null;

            if (request.Photo != null)
            {
                photoUrl = await storage.UploadPhotoAsync(request.Photo);
            }
            else
            {
                photoUrl = "https://storage.yandexcloud.net/congratulator-photos/default.png";
            }

            //Create
            var person = new Person
            {
                Name = request.Name,
                BirthDate = request.BirthDate,
                RelationshipType = request.RelationshipType,
                PhotoUrl = photoUrl,
                Email = request.Email
            };
            await repository.CreatePersonAsync(person);

            //Response
            return new PersonResponse
            {
                Guid = person.Guid,
                Name = person.Name,
                BirthDate = person.BirthDate,
                RelationshipType = person.RelationshipType,
                PhotoUrl = person.PhotoUrl
            };
        }
    }
}
