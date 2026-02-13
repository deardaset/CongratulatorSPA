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
    public class PersonRepository(AppDbContext context, IMapper mapper) : IPersonRepository
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

        public async Task<(List<PersonModel>, int totalCount)> GetAllPeopleAsync()
        {
            var query = context.People.AsQueryable();

            var totalCount = query.Count();

            var people = mapper.Map<List<PersonModel>>(await query.ToListAsync());

            return (people, totalCount);
        }
        
        public async Task<PersonModel> GetPersonByIdAsync(Guid guid)
        {
            var person = mapper.Map<PersonModel>(await context.People.FirstOrDefaultAsync(p => p.Guid == guid));
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
