using Amazon.Runtime.Internal;
using AutoMapper;
using CongratulatorSPA.Server.Entities;
using CongratulatorSPA.Server.Exceptions;
using CongratulatorSPA.Server.Interfaces.Repositories;
using CongratulatorSPA.Server.Interfaces.Services;
using CongratulatorSPA.Server.Models;
using CongratulatorSPA.Server.Models.Enums;
using CongratulatorSPA.Server.Models.Requests;
using CongratulatorSPA.Server.Models.Responses;
using System.Xml.Linq;

namespace CongratulatorSPA.Server.Services
{
    public class GetPeopleService(IPersonRepository repository, IMapper mapper) : IGetPeopleService<PersonResponse>
    {
        public async Task<PagedResponse<PersonResponse>> RunAsync(GetPeopleOptionsRequest request)
        {
            if (request.Page <= 0 || request.PageSize <= 0)
                throw new BadRequestException("Invalid pagination parameters");

            var (people, totalCount) = await repository.GetAllPeopleAsync(request);
            var peopleModels = mapper.Map<List<PersonModel>>(people);

            return new PagedResponse<PersonResponse>
            {
                Data = peopleModels.Select(p => new PersonResponse
                {
                    Guid = p.Guid,
                    Name = p.Name,
                    BirthDate = p.BirthDate,
                    RelationshipType = p.RelationshipType,
                    PhotoUrl = p.PhotoUrl,
                    Age = p.Age
                }).ToList(),
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
