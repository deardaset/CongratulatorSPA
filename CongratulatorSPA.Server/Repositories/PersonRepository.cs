using CongratulatorSPA.Server.Data;
using CongratulatorSPA.Server.Entities;
using CongratulatorSPA.Server.Exceptions;
using CongratulatorSPA.Server.Interfaces.Repositories;
using CongratulatorSPA.Server.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace CongratulatorSPA.Server.Repositories
{
    public class PersonRepository(AppDbContext context) : IPersonRepository
    {
        public async Task CreatePersonAsync(Person person)
        {
            context.People.Add(person);
            await context.SaveChangesAsync();
        }

        public async Task DeletePersonAsync(Person person)
        {
            context.People.Remove(person);
            await context.SaveChangesAsync();
        }

        public async Task<(List<Person>, int totalCount)> GetAllPeopleAsync(int page, int pageSize)
        {
            var query = context.People.AsQueryable();

            var totalCount = await query.CountAsync();

            var people = await query
                .OrderBy(p => p.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (people, totalCount);
        }

        public async Task<Person> GetPersonByIdAsync(Guid guid)
        {
            var person = await context.People.FirstOrDefaultAsync(p => p.Guid == guid);
            if (person == null)
                throw new NotFoundException("Person not found");
            return person;
        }

        public async Task UpdatePersonAsync(Person person)
        {
            context.People.Update(person);
            await context.SaveChangesAsync();
        }
    }
}
