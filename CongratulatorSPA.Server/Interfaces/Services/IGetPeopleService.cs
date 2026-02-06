using CongratulatorSPA.Server.Entities;
using CongratulatorSPA.Server.Models.Responses;

namespace CongratulatorSPA.Server.Interfaces.Services
{
    public interface IGetPeopleService
    {
        public Task<PagedResponse<PersonResponse>> RunAsync(int page, int pageSize);
    }
}
