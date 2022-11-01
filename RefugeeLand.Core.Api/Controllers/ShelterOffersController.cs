using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using RefugeeLand.Core.Api.Models.ShelterOffers;
using RefugeeLand.Core.Api.Models.ShelterOffers.Exceptions;
using RefugeeLand.Core.Api.Services.Foundations.ShelterOffers;

namespace RefugeeLand.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShelterOffersController : RESTFulController
    {
        private readonly IShelterOfferService shelterOfferService;

        public ShelterOffersController(IShelterOfferService shelterOfferService) =>
            this.shelterOfferService = shelterOfferService;

        [HttpPost]
        public async ValueTask<ActionResult<ShelterOffer>> PostShelterOfferAsync(ShelterOffer shelterOffer)
        {
            try
            {
                ShelterOffer addedShelterOffer =
                    await this.shelterOfferService.AddShelterOfferAsync(shelterOffer);

                return Created(addedShelterOffer);
            }
            catch (ShelterOfferValidationException shelterOfferValidationException)
            {
                return BadRequest(shelterOfferValidationException.InnerException);
            }
            catch (ShelterOfferDependencyValidationException shelterOfferValidationException)
                when (shelterOfferValidationException.InnerException is InvalidShelterOfferReferenceException)
            {
                return FailedDependency(shelterOfferValidationException.InnerException);
            }
            catch (ShelterOfferDependencyValidationException shelterOfferDependencyValidationException)
               when (shelterOfferDependencyValidationException.InnerException is AlreadyExistsShelterOfferException)
            {
                return Conflict(shelterOfferDependencyValidationException.InnerException);
            }
            catch (ShelterOfferDependencyException shelterOfferDependencyException)
            {
                return InternalServerError(shelterOfferDependencyException);
            }
            catch (ShelterOfferServiceException shelterOfferServiceException)
            {
                return InternalServerError(shelterOfferServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<ShelterOffer>> GetAllShelterOffers()
        {
            try
            {
                IQueryable<ShelterOffer> retrievedShelterOffers =
                    this.shelterOfferService.RetrieveAllShelterOffers();

                return Ok(retrievedShelterOffers);
            }
            catch (ShelterOfferDependencyException shelterOfferDependencyException)
            {
                return InternalServerError(shelterOfferDependencyException);
            }
            catch (ShelterOfferServiceException shelterOfferServiceException)
            {
                return InternalServerError(shelterOfferServiceException);
            }
        }

        [HttpGet("{shelterOfferId}")]
        public async ValueTask<ActionResult<ShelterOffer>> GetShelterOfferByIdAsync(Guid shelterOfferId)
        {
            try
            {
                ShelterOffer shelterOffer = await this.shelterOfferService.RetrieveShelterOfferByIdAsync(shelterOfferId);

                return Ok(shelterOffer);
            }
            catch (ShelterOfferValidationException shelterOfferValidationException)
                when (shelterOfferValidationException.InnerException is NotFoundShelterOfferException)
            {
                return NotFound(shelterOfferValidationException.InnerException);
            }
            catch (ShelterOfferValidationException shelterOfferValidationException)
            {
                return BadRequest(shelterOfferValidationException.InnerException);
            }
            catch (ShelterOfferDependencyException shelterOfferDependencyException)
            {
                return InternalServerError(shelterOfferDependencyException);
            }
            catch (ShelterOfferServiceException shelterOfferServiceException)
            {
                return InternalServerError(shelterOfferServiceException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<ShelterOffer>> PutShelterOfferAsync(ShelterOffer shelterOffer)
        {
            try
            {
                ShelterOffer modifiedShelterOffer =
                    await this.shelterOfferService.ModifyShelterOfferAsync(shelterOffer);

                return Ok(modifiedShelterOffer);
            }
            catch (ShelterOfferValidationException shelterOfferValidationException)
                when (shelterOfferValidationException.InnerException is NotFoundShelterOfferException)
            {
                return NotFound(shelterOfferValidationException.InnerException);
            }
            catch (ShelterOfferValidationException shelterOfferValidationException)
            {
                return BadRequest(shelterOfferValidationException.InnerException);
            }
            catch (ShelterOfferDependencyValidationException shelterOfferValidationException)
                when (shelterOfferValidationException.InnerException is InvalidShelterOfferReferenceException)
            {
                return FailedDependency(shelterOfferValidationException.InnerException);
            }
            catch (ShelterOfferDependencyValidationException shelterOfferDependencyValidationException)
               when (shelterOfferDependencyValidationException.InnerException is AlreadyExistsShelterOfferException)
            {
                return Conflict(shelterOfferDependencyValidationException.InnerException);
            }
            catch (ShelterOfferDependencyException shelterOfferDependencyException)
            {
                return InternalServerError(shelterOfferDependencyException);
            }
            catch (ShelterOfferServiceException shelterOfferServiceException)
            {
                return InternalServerError(shelterOfferServiceException);
            }
        }

        [HttpDelete("{shelterOfferId}")]
        public async ValueTask<ActionResult<ShelterOffer>> DeleteShelterOfferByIdAsync(Guid shelterOfferId)
        {
            try
            {
                ShelterOffer deletedShelterOffer =
                    await this.shelterOfferService.RemoveShelterOfferByIdAsync(shelterOfferId);

                return Ok(deletedShelterOffer);
            }
            catch (ShelterOfferValidationException shelterOfferValidationException)
                when (shelterOfferValidationException.InnerException is NotFoundShelterOfferException)
            {
                return NotFound(shelterOfferValidationException.InnerException);
            }
            catch (ShelterOfferValidationException shelterOfferValidationException)
            {
                return BadRequest(shelterOfferValidationException.InnerException);
            }
            catch (ShelterOfferDependencyValidationException shelterOfferDependencyValidationException)
                when (shelterOfferDependencyValidationException.InnerException is LockedShelterOfferException)
            {
                return Locked(shelterOfferDependencyValidationException.InnerException);
            }
            catch (ShelterOfferDependencyValidationException shelterOfferDependencyValidationException)
            {
                return BadRequest(shelterOfferDependencyValidationException);
            }
            catch (ShelterOfferDependencyException shelterOfferDependencyException)
            {
                return InternalServerError(shelterOfferDependencyException);
            }
            catch (ShelterOfferServiceException shelterOfferServiceException)
            {
                return InternalServerError(shelterOfferServiceException);
            }
        }
    }
}