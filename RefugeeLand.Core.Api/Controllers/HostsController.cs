using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using RefugeeLand.Core.Api.Models.Hosts;
using RefugeeLand.Core.Api.Models.Hosts.Exceptions;
using RefugeeLand.Core.Api.Services.Foundations.Hosts;

namespace RefugeeLand.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HostsController : RESTFulController
    {
        private readonly IHostService hostService;

        public HostsController(IHostService hostService) =>
            this.hostService = hostService;

        [HttpPost]
        public async ValueTask<ActionResult<Host>> PostHostAsync(Host host)
        {
            try
            {
                Host addedHost =
                    await this.hostService.AddHostAsync(host);

                return Created(addedHost);
            }
            catch (HostValidationException hostValidationException)
            {
                return BadRequest(hostValidationException.InnerException);
            }
            catch (HostDependencyValidationException hostValidationException)
                when (hostValidationException.InnerException is InvalidHostReferenceException)
            {
                return FailedDependency(hostValidationException.InnerException);
            }
            catch (HostDependencyValidationException hostDependencyValidationException)
               when (hostDependencyValidationException.InnerException is AlreadyExistsHostException)
            {
                return Conflict(hostDependencyValidationException.InnerException);
            }
            catch (HostDependencyException hostDependencyException)
            {
                return InternalServerError(hostDependencyException);
            }
            catch (HostServiceException hostServiceException)
            {
                return InternalServerError(hostServiceException);
            }
        }
    }
}