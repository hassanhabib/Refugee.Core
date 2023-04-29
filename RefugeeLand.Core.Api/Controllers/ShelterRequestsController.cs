using System;
using System.Linq;
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

        [HttpGet]
        public ActionResult<IQueryable<ShelterRequest>> GetAllShelterRequests()
        {
            try
            {
                IQueryable<ShelterRequest> retrievedShelterRequests =
                    this.shelterRequestService.RetrieveAllShelterRequests();

                return Ok(retrievedShelterRequests);
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

        [HttpGet("{shelterRequestId}")]
        public async ValueTask<ActionResult<ShelterRequest>> GetShelterRequestByIdAsync(Guid shelterRequestId)
        {
            try
            {
                ShelterRequest shelterRequest = await this.shelterRequestService.RetrieveShelterRequestByIdAsync(shelterRequestId);

                return Ok(shelterRequest);
            }
            catch (ShelterRequestValidationException shelterRequestValidationException)
                when (shelterRequestValidationException.InnerException is NotFoundShelterRequestException)
            {
                return NotFound(shelterRequestValidationException.InnerException);
            }
            catch (ShelterRequestValidationException shelterRequestValidationException)
            {
                return BadRequest(shelterRequestValidationException.InnerException);
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

        [HttpPut]
        public async ValueTask<ActionResult<ShelterRequest>> PutShelterRequestAsync(ShelterRequest shelterRequest)
        {
            try
            {
                ShelterRequest modifiedShelterRequest =
                    await this.shelterRequestService.ModifyShelterRequestAsync(shelterRequest);

                return Ok(modifiedShelterRequest);
            }
            catch (ShelterRequestValidationException shelterRequestValidationException)
                when (shelterRequestValidationException.InnerException is NotFoundShelterRequestException)
            {
                return NotFound(shelterRequestValidationException.InnerException);
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

        [HttpDelete("{shelterRequestId}")]
        public async ValueTask<ActionResult<ShelterRequest>> DeleteShelterRequestByIdAsync(Guid shelterRequestId)
        {
            try
            {
                ShelterRequest deletedShelterRequest =
                    await this.shelterRequestService.RemoveShelterRequestByIdAsync(shelterRequestId);

                return Ok(deletedShelterRequest);
            }
            catch (ShelterRequestValidationException shelterRequestValidationException)
                when (shelterRequestValidationException.InnerException is NotFoundShelterRequestException)
            {
                return NotFound(shelterRequestValidationException.InnerException);
            }
            catch (ShelterRequestValidationException shelterRequestValidationException)
            {
                return BadRequest(shelterRequestValidationException.InnerException);
            }
            catch (ShelterRequestDependencyValidationException shelterRequestDependencyValidationException)
                when (shelterRequestDependencyValidationException.InnerException is LockedShelterRequestException)
            {
                return Locked(shelterRequestDependencyValidationException.InnerException);
            }
            catch (ShelterRequestDependencyValidationException shelterRequestDependencyValidationException)
            {
                return BadRequest(shelterRequestDependencyValidationException);
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