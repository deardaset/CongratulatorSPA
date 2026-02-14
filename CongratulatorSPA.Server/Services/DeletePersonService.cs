using AutoMapper;
using CongratulatorSPA.Server.Entities;
using CongratulatorSPA.Server.Exceptions;
using CongratulatorSPA.Server.Interfaces.Repositories;
using CongratulatorSPA.Server.Interfaces.Services;

namespace CongratulatorSPA.Server.Services
{
    public class DeletePersonService(IPersonRepository repository, IStorageService storage) : IDeletePersonService
    {
        public async Task RunAsync(Guid guid)
        {
            var person = await repository.GetPersonByIdAsync(guid);
            if (person == null)
                throw new NotFoundException("Person not found");

            if (person.PhotoUrl != "https://storage.yandexcloud.net/congratulator-photos/default.png")
                await storage.DeletePhotoAsync(person.PhotoUrl);

            await repository.DeletePersonAsync(person);
        }
    }
}
