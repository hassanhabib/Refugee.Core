using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using RefugeeLand.Core.Api.Models.Nationalities;
using RefugeeLand.Core.Api.Models.Nationalities.Exceptions;
using RefugeeLand.Core.Api.Services.Foundations.Nationalities;

namespace RefugeeLand.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NationalitiesController : RESTFulController
    {
        private readonly INationalityService nationalityService;

        public NationalitiesController(INationalityService nationalityService) =>
            this.nationalityService = nationalityService;

        [HttpPost]
        public async ValueTask<ActionResult<Nationality>> PostNationalityAsync(Nationality nationality)
        {
            try
            {
                Nationality addedNationality =
                    await this.nationalityService.AddNationalityAsync(nationality);

                return Created(addedNationality);
            }
            catch (NationalityValidationException nationalityValidationException)
            {
                return BadRequest(nationalityValidationException.InnerException);
            }
            catch (NationalityDependencyValidationException nationalityValidationException)
                when (nationalityValidationException.InnerException is InvalidNationalityReferenceException)
            {
                return FailedDependency(nationalityValidationException.InnerException);
            }
            catch (NationalityDependencyValidationException nationalityDependencyValidationException)
               when (nationalityDependencyValidationException.InnerException is AlreadyExistsNationalityException)
            {
                return Conflict(nationalityDependencyValidationException.InnerException);
            }
            catch (NationalityDependencyException nationalityDependencyException)
            {
                return InternalServerError(nationalityDependencyException);
            }
            catch (NationalityServiceException nationalityServiceException)
            {
                return InternalServerError(nationalityServiceException);
            }
        }
    }
}