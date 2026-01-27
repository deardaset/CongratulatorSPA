using CongratulatorSPA.Server.Models.Requests;
using CongratulatorSPA.Server.Models.Responses;

namespace CongratulatorSPA.Server.Interfaces.Services
{
    public interface ICreatePersonService
    {
        public Task<PersonResponse> RunAsync(CreatePersonRequest request);
    }
}
