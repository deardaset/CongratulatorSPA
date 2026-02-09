using CongratulatorSPA.Server.Entities;
using CongratulatorSPA.Server.Models.Requests;
using CongratulatorSPA.Server.Models.Responses;

namespace CongratulatorSPA.Server.Interfaces.Repositories
{
    public interface IPersonRepository
    {
        public Task CreatePersonAsync(Person person);
        public Task<(List<Person>, int totalCount)> GetAllPeopleAsync();
        public Task<Person> GetPersonByIdAsync(Guid guid);
        public Task UpdatePersonAsync(Person person);
        public Task DeletePersonAsync(Person person);
    }
}
