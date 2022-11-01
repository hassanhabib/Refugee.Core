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
    }
}