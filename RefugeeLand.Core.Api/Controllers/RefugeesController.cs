// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RefugeeLand.Core.Api.Models.Refugees;
using RefugeeLand.Core.Api.Models.Refugees.Exceptions;
using RefugeeLand.Core.Api.Services.Foundations.Refugees;
using RESTFulSense.Controllers;

namespace RefugeeLand.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RefugeesController : RESTFulController
    {
        private readonly IRefugeeService refugeeService;

        public RefugeesController(IRefugeeService refugeeService) =>
            this.refugeeService = refugeeService;

        [HttpPost]
        public async ValueTask<ActionResult<Refugee>> PostRefugeeAsync(Refugee refugee)
        {
            try
            {
                Refugee postedRefugee = await this.refugeeService.AddRefugeeAsync(refugee);

                return Created(postedRefugee);
            }
            catch (RefugeeValidationException refugeeValidationException)
            {
                return BadRequest(refugeeValidationException.InnerException);
            }
            catch (RefugeeDependencyValidationException refugeeDependencyValidationException)
                when (refugeeDependencyValidationException.InnerException is AlreadyExistRefugeeException)
            {
                return Conflict(refugeeDependencyValidationException.InnerException);
            }
            catch (RefugeeDependencyValidationException refugeeDependencyValidationException)
            {
                return BadRequest(refugeeDependencyValidationException.InnerException);
            }
            catch (RefugeeDependencyException refugeeDependencyException)
            {
                return InternalServerError(refugeeDependencyException.InnerException.Message);
            }
            catch (RefugeeServiceException refugeeServiceException)
            {
                return InternalServerError(refugeeServiceException);
            }
        }
    }
}
