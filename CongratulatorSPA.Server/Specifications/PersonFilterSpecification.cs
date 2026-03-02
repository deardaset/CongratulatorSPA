using CongratulatorSPA.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace CongratulatorSPA.Server.Specifications
{
    public class PersonFilterSpecification
    {
        const int ADD_DAYS = 30;
        public IQueryable<Person> Apply(IQueryable<Person> query, string? search, string? sort, bool upcoming)
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

            if (upcoming)
            {
                var endDate = today.AddDays(ADD_DAYS);

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

            return query;
        }
    }
}
