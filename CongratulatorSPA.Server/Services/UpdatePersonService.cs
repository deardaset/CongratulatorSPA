using AutoMapper;
using CongratulatorSPA.Server.Entities;
using CongratulatorSPA.Server.Exceptions;
using CongratulatorSPA.Server.Interfaces.Repositories;
using CongratulatorSPA.Server.Interfaces.Services;
using CongratulatorSPA.Server.Models.Requests;
using CongratulatorSPA.Server.Models.Responses;
using System.Text.RegularExpressions;

namespace CongratulatorSPA.Server.Services
{
    public class UpdatePersonService(IPersonRepository repository, IStorageService storage) : IUpdatePersonService
    {
        public async Task<PersonResponse> RunAsync(Guid guid, UpdatePersonRequest request)
        {
            var person = await repository.GetPersonByIdAsync(guid);
            if (person == null)
                throw new NotFoundException("Person not found");

            string? photoUrl = null;

            if (request.Photo != null)
            {
                photoUrl = await storage.UploadPhotoAsync(request.Photo);
                if (person.PhotoUrl != "https://storage.yandexcloud.net/congratulator-photos/default.png")
                    await storage.DeletePhotoAsync(person.PhotoUrl);
                
            }

            bool hasChanges = false;

            if (request.Name is { } name && name != person.Name)
                (person.Name, hasChanges) = (name, true);
            if (request.BirthDate is { } date && date != person.BirthDate)
                (person.BirthDate, hasChanges) = (date, true);
            if (request.RelationshipType is { } type && type != person.RelationshipType)
                (person.RelationshipType, hasChanges) = (type, true);
            if (photoUrl is { } url && url != person.PhotoUrl)
                (person.PhotoUrl, hasChanges) = (url, true);
            
            if (hasChanges)
            {
                await repository.UpdatePersonAsync(person);
            }

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
