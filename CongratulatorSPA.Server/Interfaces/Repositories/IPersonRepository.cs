using CongratulatorSPA.Server.Entities;
using CongratulatorSPA.Server.Models;
using CongratulatorSPA.Server.Models.Requests;
using CongratulatorSPA.Server.Models.Responses;

namespace CongratulatorSPA.Server.Interfaces.Repositories
{
    public interface IPersonRepository
    {
        public Task CreatePersonAsync(Person person);
        public Task<(List<Person>, int totalCount)> GetAllPeopleAsync(int page, int pageSize, string? search, string? sort, bool upcoming);
        public Task<Person> GetPersonByIdAsync(Guid guid);
        public Task UpdatePersonAsync(Person person);
        public Task DeletePersonAsync(Person person);
    }
}
