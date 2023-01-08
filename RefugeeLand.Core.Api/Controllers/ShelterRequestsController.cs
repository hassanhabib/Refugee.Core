using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using RefugeeLand.Core.Api.Models.ShelterRequests;
using RefugeeLand.Core.Api.Models.ShelterRequests.Exceptions;
using RefugeeLand.Core.Api.Services.Foundations.ShelterRequests;

namespace RefugeeLand.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShelterRequestsController : RESTFulController
    {
        private readonly IShelterRequestService shelterRequestService;

        public ShelterRequestsController(IShelterRequestService shelterRequestService) =>
            this.shelterRequestService = shelterRequestService;

        [HttpPost]
        public async ValueTask<ActionResult<ShelterRequest>> PostShelterRequestAsync(ShelterRequest shelterRequest)
        {
            try
            {
                ShelterRequest addedShelterRequest =
                    await this.shelterRequestService.AddShelterRequestAsync(shelterRequest);

                return Created(addedShelterRequest);
            }
            catch (ShelterRequestValidationException shelterRequestValidationException)
            {
                return BadRequest(shelterRequestValidationException.InnerException);
            }
            catch (ShelterRequestDependencyValidationException shelterRequestValidationException)
                when (shelterRequestValidationException.InnerException is InvalidShelterRequestReferenceException)
            {
                return FailedDependency(shelterRequestValidationException.InnerException);
            }
            catch (ShelterRequestDependencyValidationException shelterRequestDependencyValidationException)
               when (shelterRequestDependencyValidationException.InnerException is AlreadyExistsShelterRequestException)
            {
                return Conflict(shelterRequestDependencyValidationException.InnerException);
            }
            catch (ShelterRequestDependencyException shelterRequestDependencyException)
            {
                return InternalServerError(shelterRequestDependencyException);
            }
            catch (ShelterRequestServiceException shelterRequestServiceException)
            {
                return InternalServerError(shelterRequestServiceException);
            }
        }
    }
}