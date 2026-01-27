using CongratulatorSPA.Server.Models.Requests;
using CongratulatorSPA.Server.Models.Responses;

namespace CongratulatorSPA.Server.Interfaces.Services
{
    public interface IUpdatePersonService
    {
        public Task<PersonResponse> RunAsync(Guid guid,UpdatePersonRequest request);
    }
}
