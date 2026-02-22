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

        public async Task<(List<Person>, int totalCount)> GetAllPeopleAsync(GetPeopleOptionsRequest request)
        {
            var query = context.People.AsNoTracking().AsQueryable();

            query = ApplyFilters(query, request.SearchBy, request.SortBy);
            if (request.Upcoming)
                query = IsUpcoming(query);

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

        public async Task<Person> GetPersonByIdAsync(Guid guid)
        {
            var person = await context.People.FirstOrDefaultAsync(p => p.Guid == guid);
            return person ?? throw new PersonNotFoundException("Person not found");
        }

        public async Task UpdatePersonAsync(Person person)
        {
            context.People.Update(person);
            await context.SaveChangesAsync();
        }
        //Additional
        public static IQueryable<Person> ApplyFilters(IQueryable<Person> query, string? search, string? sort)
        {
            if (!string.IsNullOrEmpty(search))
            {
                var term = $"%{search.Trim()}%";

                query = query.Where(p =>
                    EF.Functions.ILike(p.Name, term) ||
                    EF.Functions.ILike(p.RelationshipType.ToString(), term)
                );
            }

            var today = DateTime.Today;

            query = sort?.ToLower() switch
            {
                "name" => query.OrderBy(p => p.Name),
                "age" => query.OrderBy(p => p.BirthDate),
                "nextbirthday" => query.OrderBy(p =>
                    new DateTime(today.Year, p.BirthDate.Month, p.BirthDate.Day) < today
                        ? new DateTime(today.Year + 1, p.BirthDate.Month, p.BirthDate.Day)
                        : new DateTime(today.Year, p.BirthDate.Month, p.BirthDate.Day)),
                "relationship" => query.OrderBy(p => p.RelationshipType),
                _ => query.OrderBy(p => p.Name)
            };
            return query;
        }
        private static IQueryable<Person> IsUpcoming(IQueryable<Person> query)
        {
            var today = DateTime.Today;
            var endDate = today.AddDays(30);

            return query.Where(p =>
                (
                    new DateTime(today.Year, p.BirthDate.Month, p.BirthDate.Day) < today
                        ? new DateTime(today.Year + 1, p.BirthDate.Month, p.BirthDate.Day)
                        : new DateTime(today.Year, p.BirthDate.Month, p.BirthDate.Day)
                ) >= today
                &&
                (
                    new DateTime(today.Year, p.BirthDate.Month, p.BirthDate.Day) < today
                        ? new DateTime(today.Year + 1, p.BirthDate.Month, p.BirthDate.Day)
                        : new DateTime(today.Year, p.BirthDate.Month, p.BirthDate.Day)
                ) <= endDate
            );
        }
    }
}
