using CongratulatorSPA.Server.Entities;
using CongratulatorSPA.Server.Models.Requests;

namespace CongratulatorSPA.Server.Interfaces.Repositories
{
    public interface IPersonRepository
    {
        public Task CreatePersonAsync(Person person);
        public Task<List<Person>> GetAllPeopleAsync();
        public Task<Person> GetPersonByIdAsync(Guid guid);
        public Task UpdatePersonAsync(Person person);
        public Task DeletePersonAsync(Person person);
    }
}
