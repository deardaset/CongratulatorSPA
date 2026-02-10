using CongratulatorSPA.Server.Entities;
using CongratulatorSPA.Server.Interfaces.Services;
using CongratulatorSPA.Server.Models.Requests;
using CongratulatorSPA.Server.Models.Responses;
using CongratulatorSPA.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace CongratulatorSPA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreatePersonAsync([FromServices] ICreatePersonService service, [FromForm] CreatePersonRequest request)
        {
            var result = await service.RunAsync(request);
            return Ok(result);
        }

        [HttpPut]
        [Route("{guid}")]
        public async Task<IActionResult> UpdatePersonAsync([FromRoute] Guid guid, [FromServices] IUpdatePersonService service,[FromForm] UpdatePersonRequest request)
        {
            var result = await service.RunAsync(guid, request);
            return Ok(result);
        }

        [HttpDelete]
        [Route("{guid}")]
        public async Task<IActionResult> DeletePersonAsync([FromRoute] Guid guid, [FromServices] IDeletePersonService service)
        {
            await service.RunAsync(guid);
            return Ok();
        }

        [HttpGet]
        [Route("{guid}")]
        public async Task<Person> GetPersonAsync([FromRoute] Guid guid, [FromServices] IGetPersonService service)
        {
            var result = await service.RunAsync(guid);
            return result;
        }

        [HttpGet]
        public async Task<IActionResult> GetPeopleAsync(
            [FromServices] IGetPeopleService<PersonResponse> service,
            [FromQuery] GetPeopleOptionsRequest request)
        {
            var result = await service.RunAsync(request);
            return Ok(result);
        }

        [HttpGet("main")]
        public async Task<IActionResult> GetUpcomingAsync([FromServices] IGetUpcomingService<PersonResponse> service, [FromQuery] GetPeopleOptionsRequest request)
        {
            var result = await service.RunAsync(request);
            return Ok(result);
        }
    }
}
