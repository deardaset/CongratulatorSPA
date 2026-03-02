using CongratulatorSPA.Server.Data;
using CongratulatorSPA.Server.Entities;
using CongratulatorSPA.Server.Exceptions;
using CongratulatorSPA.Server.Interfaces.Repositories;
using CongratulatorSPA.Server.Models.Requests;
using CongratulatorSPA.Server.Specifications;
using EFCoreSecondLevelCacheInterceptor;
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

        public async Task<(List<Person>, int totalCount)> GetAllPeopleAsync(GetPeopleOptionsRequest request)
        {
            var query = context.People.AsNoTracking().AsQueryable();

            var spec = new PersonFilterSpecification();
            query = spec.Apply(query, request.SearchBy, request.SortBy, request.Upcoming);

            var totalCount = await query.CountAsync();

            var people = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return (people, totalCount);
        }
        
        public async Task<List<Person>> GetTodaysBirthdaysAsync(CancellationToken cancellationToken)
        {
            var today = DateTime.Today;
            var query = context.People.AsNoTracking().Where(p => p.BirthDate.Month == today.Month && p.BirthDate.Day == today.Day && !string.IsNullOrEmpty(p.Email));

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Person?> GetPersonByIdAsync(Guid guid)
        {
            return await context.People.FirstOrDefaultAsync(p => p.Guid == guid);
        }

        public async Task UpdatePersonAsync(Person person)
        {
            context.People.Update(person);
            await context.SaveChangesAsync();
        }
    }
}
