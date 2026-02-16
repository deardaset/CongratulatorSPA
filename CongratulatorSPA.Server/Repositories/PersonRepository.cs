using AutoMapper;
using CongratulatorSPA.Server.Data;
using CongratulatorSPA.Server.Entities;
using CongratulatorSPA.Server.Exceptions;
using CongratulatorSPA.Server.Interfaces.Repositories;
using CongratulatorSPA.Server.Models;
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

        public async Task<(List<Person>, int totalCount)> GetAllPeopleAsync()
        {
            var query = context.People.AsQueryable();

            var totalCount = await query.CountAsync();

            var people = await query.ToListAsync();

            return (people, totalCount);
        }
        
        public async Task<Person> GetPersonByIdAsync(Guid guid)
        {
            var person = await context.People.FirstOrDefaultAsync(p => p.Guid == guid);
            return person ?? throw new NotFoundException("Person not found");
        }

        public async Task UpdatePersonAsync(Person person)
        {
            context.People.Update(person);
            await context.SaveChangesAsync();
        }
    }
}
