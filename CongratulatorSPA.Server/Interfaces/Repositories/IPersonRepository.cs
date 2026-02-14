using CongratulatorSPA.Server.Entities;
using CongratulatorSPA.Server.Models;
using CongratulatorSPA.Server.Models.Requests;
using CongratulatorSPA.Server.Models.Responses;

namespace CongratulatorSPA.Server.Interfaces.Repositories
{
    public interface IPersonRepository
    {
        public Task CreatePersonAsync(Person person);
        public Task<(List<PersonModel>, int totalCount)> GetAllPeopleAsync();
        public Task<PersonModel> GetPersonByIdAsync(Guid guid);
        public Task UpdatePersonAsync(PersonModel person);
        public Task DeletePersonAsync(PersonModel person);
    }
}
