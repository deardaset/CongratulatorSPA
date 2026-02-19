using Microsoft.AspNetCore.Diagnostics;

namespace CongratulatorSPA.Server.Exceptions
{
    public class CongratulatorExceptionHandler(ILogger<CongratulatorExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpcontext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, "Exception occured: {Message}", exception.Message);

            var statuscode = exception switch
            {
                CongratulatorException x => x.StatusCode,
                _ => StatusCodes.Status500InternalServerError
            };
            httpcontext.Response.StatusCode = statuscode;

            var error = new
            {
                Id = Guid.NewGuid(),
                StatusCode = statuscode,
                ErrorMessage = exception.Message
            };

            await httpcontext.Response.WriteAsJsonAsync(error, cancellationToken);

            return true;
        }
    }
}
