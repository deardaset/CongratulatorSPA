using Amazon.Runtime.Internal;
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

        public async Task<(List<Person>, int totalCount)> GetAllPeopleAsync(int page, int pageSize, string? search, string? sort, bool upcoming)
        {
            var query = context.People.AsQueryable();

            query = ApplyFilters(query, search, sort);
            if (upcoming)
                query = IsUpcoming(query);

            var totalCount = await query.CountAsync();

            var people = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (people, totalCount);
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
                search = search.ToLower();
                query = query.Where(p =>
                    p.Name.ToLower().Contains(search) ||
                    p.RelationshipType.ToString().ToLower().Contains(search));
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
