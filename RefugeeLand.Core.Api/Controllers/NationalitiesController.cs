using System;
using System.Linq;
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

        [HttpGet]
        public ActionResult<IQueryable<Nationality>> GetAllNationalities()
        {
            try
            {
                IQueryable<Nationality> retrievedNationalities =
                    this.nationalityService.RetrieveAllNationalities();

                return Ok(retrievedNationalities);
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

        [HttpGet("{nationalityId}")]
        public async ValueTask<ActionResult<Nationality>> GetNationalityByIdAsync(Guid nationalityId)
        {
            try
            {
                Nationality nationality = await this.nationalityService.RetrieveNationalityByIdAsync(nationalityId);

                return Ok(nationality);
            }
            catch (NationalityValidationException nationalityValidationException)
                when (nationalityValidationException.InnerException is NotFoundNationalityException)
            {
                return NotFound(nationalityValidationException.InnerException);
            }
            catch (NationalityValidationException nationalityValidationException)
            {
                return BadRequest(nationalityValidationException.InnerException);
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