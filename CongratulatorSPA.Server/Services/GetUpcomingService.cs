using AutoMapper;
using CongratulatorSPA.Server.Exceptions;
using CongratulatorSPA.Server.Interfaces.Repositories;
using CongratulatorSPA.Server.Interfaces.Services;
using CongratulatorSPA.Server.Models;
using CongratulatorSPA.Server.Models.Requests;
using CongratulatorSPA.Server.Models.Responses;

namespace CongratulatorSPA.Server.Services
{
    public class GetUpcomingService(IPersonRepository repository, IMapper mapper) : IGetUpcomingService<PersonResponse>
    {
        const int ADD_DAYS = 30;
        public async Task<PagedResponse<PersonResponse>> RunAsync(GetPeopleOptionsRequest request)
        {
            if (request.Page <= 0 || request.PageSize <= 0)
                throw new BadRequestException("Invalid pagination parameters");

            var (people, totalCount) = await repository.GetAllPeopleAsync();
            var peopleModels = mapper.Map<List<PersonModel>>(people);

            var sortBy = request.SortBy?.ToLower() ?? string.Empty;

            var sortedPeople = sortBy switch
            {
                "name" => peopleModels.OrderBy(p => p.Name),
                "birthdate" => peopleModels.OrderBy(p => p.NextBirthday),
                "age" => peopleModels.OrderBy(p => p.Age),
                "relationship" => peopleModels.OrderBy(p => p.RelationshipType),
                _ => peopleModels.OrderBy(p => p.Name)
            };

            var searchedPeople = sortedPeople.Where(p =>
                string.IsNullOrWhiteSpace(request.SearchBy) ||
                p.Name.Contains(request.SearchBy, StringComparison.OrdinalIgnoreCase) ||
                p.RelationshipType.ToString().Contains(request.SearchBy ?? string.Empty, StringComparison.OrdinalIgnoreCase)
            );

            var filteredPeople = searchedPeople.Where(p => p.NextBirthday >= DateTime.Today && p.NextBirthday <= DateTime.Today.AddDays(ADD_DAYS));

            var count = filteredPeople.ToList();

            var paginatedPeople = filteredPeople
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
                TotalCount = count.Count,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
