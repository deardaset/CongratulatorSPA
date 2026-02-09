using CongratulatorSPA.Server.Models.Requests;
using CongratulatorSPA.Server.Models.Responses;

namespace CongratulatorSPA.Server.Interfaces.Services
{
    public interface IGetUpcomingService<T>
    {
        public Task<PagedResponse<T>> RunAsync(GetPeopleOptionsRequest request);
    }
}
