using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using RefugeeLand.Core.Api.Models.RefugeeGroups;
using RefugeeLand.Core.Api.Models.RefugeeGroups.Exceptions;
using RefugeeLand.Core.Api.Services.Foundations.RefugeeGroups;

namespace RefugeeLand.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RefugeeGroupsController : RESTFulController
    {
        private readonly IRefugeeGroupService refugeeGroupService;

        public RefugeeGroupsController(IRefugeeGroupService refugeeGroupService) =>
            this.refugeeGroupService = refugeeGroupService;

        [HttpPost]
        public async ValueTask<ActionResult<RefugeeGroup>> PostRefugeeGroupAsync(RefugeeGroup refugeeGroup)
        {
            try
            {
                RefugeeGroup addedRefugeeGroup =
                    await this.refugeeGroupService.AddRefugeeGroupAsync(refugeeGroup);

                return Created(addedRefugeeGroup);
            }
            catch (RefugeeGroupValidationException refugeeGroupValidationException)
            {
                return BadRequest(refugeeGroupValidationException.InnerException);
            }
            catch (RefugeeGroupDependencyValidationException refugeeGroupValidationException)
                when (refugeeGroupValidationException.InnerException is InvalidRefugeeGroupReferenceException)
            {
                return FailedDependency(refugeeGroupValidationException.InnerException);
            }
            catch (RefugeeGroupDependencyValidationException refugeeGroupDependencyValidationException)
               when (refugeeGroupDependencyValidationException.InnerException is AlreadyExistsRefugeeGroupException)
            {
                return Conflict(refugeeGroupDependencyValidationException.InnerException);
            }
            catch (RefugeeGroupDependencyException refugeeGroupDependencyException)
            {
                return InternalServerError(refugeeGroupDependencyException);
            }
            catch (RefugeeGroupServiceException refugeeGroupServiceException)
            {
                return InternalServerError(refugeeGroupServiceException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<RefugeeGroup>> GetAllRefugeeGroups()
        {
            try
            {
                IQueryable<RefugeeGroup> retrievedRefugeeGroups =
                    this.refugeeGroupService.RetrieveAllRefugeeGroups();

                return Ok(retrievedRefugeeGroups);
            }
            catch (RefugeeGroupDependencyException refugeeGroupDependencyException)
            {
                return InternalServerError(refugeeGroupDependencyException);
            }
            catch (RefugeeGroupServiceException refugeeGroupServiceException)
            {
                return InternalServerError(refugeeGroupServiceException);
            }
        }
    }
}