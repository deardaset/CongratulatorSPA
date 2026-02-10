using CongratulatorSPA.Server.Entities;
using CongratulatorSPA.Server.Exceptions;
using CongratulatorSPA.Server.Interfaces.Repositories;
using CongratulatorSPA.Server.Interfaces.Services;
using CongratulatorSPA.Server.Models.Enums;
using CongratulatorSPA.Server.Models.Requests;
using CongratulatorSPA.Server.Models.Responses;
using System.Xml.Linq;

namespace CongratulatorSPA.Server.Services
{
    public class GetPeopleService(IPersonRepository repository) : IGetPeopleService<PersonResponse>
    {
        public async Task<PagedResponse<PersonResponse>> RunAsync(GetPeopleOptionsRequest request)
        {
            if (request.Page <= 0 || request.PageSize <= 0)
                throw new BadRequestException("Invalid pagination parameters");
            
            var (people, totalCount) = await repository.GetAllPeopleAsync();

            var sortBy = request.SortBy?.ToLower() ?? string.Empty;

            var sortedPeople = sortBy switch
            {
                "name" => people.OrderBy(p => p.Name),
                "birthdate" => people.OrderBy(p => p.NextBirthday),
                "age" => people.OrderBy(p => p.Age),
                "relationship" => people.OrderBy(p => p.RelationshipType),
                _ => people.OrderBy(p => p.Name)
            };

            var searchedPeople = sortedPeople.Where(p =>
                string.IsNullOrWhiteSpace(request.SearchBy) ||
                p.Name.Contains(request.SearchBy, StringComparison.OrdinalIgnoreCase) ||
                p.RelationshipType.ToString().Contains(request.SearchBy ?? string.Empty, StringComparison.OrdinalIgnoreCase)
            );

            var filteredPeople = searchedPeople.ToList();

            var paginatedPeople = searchedPeople
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new PagedResponse<PersonResponse>
            {
                Data = paginatedPeople.Select(p => new PersonResponse
                {
                    Guid = p.Guid,
                    Name = p.Name,
                    BirthDate = p.BirthDate,
                    RelationshipType = p.RelationshipType,
                    PhotoUrl = p.PhotoUrl,
                    Age = p.Age
                }).ToList(),
                TotalCount = filteredPeople.Count,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
