using CongratulatorSPA.Server.Entities;
using CongratulatorSPA.Server.Exceptions;
using CongratulatorSPA.Server.Interfaces.Repositories;
using CongratulatorSPA.Server.Interfaces.Services;
using CongratulatorSPA.Server.Models.Enums;
using CongratulatorSPA.Server.Models.Responses;
using System.Xml.Linq;

namespace CongratulatorSPA.Server.Services
{
    public class GetPeopleService(IPersonRepository repository) : IGetPeopleService
    {
        public async Task<PagedResponse<PersonResponse>> RunAsync(int page, int pageSize)
        {
            if (page <= 0 || pageSize <= 0)
                throw new BadRequestException("Invalid pagination parameters");

            var (people, totalCount) = await repository.GetAllPeopleAsync(page, pageSize);

            return new PagedResponse<PersonResponse>
            {
                People = people.Select(p => new PersonResponse
                {
                    Guid = p.Guid,
                    Name = p.Name,
                    BirthDate = p.BirthDate,
                    RelationshipType = p.RelationshipType
                }).ToList(),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
