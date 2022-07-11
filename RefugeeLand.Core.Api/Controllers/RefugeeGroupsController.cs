// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RefugeeLand.Core.Api.Models.RefugeeGroups;
using RefugeeLand.Core.Api.Models.RefugeeGroups.Exceptions;
using RefugeeLand.Core.Api.Services.Foundations.RefugeeGroups;
using RESTFulSense.Controllers;

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
                RefugeeGroup postedRefugeeGroup = await this.refugeeGroupService.AddRefugeeGroupAsync(refugeeGroup);

                return Created(postedRefugeeGroup);
            }
            catch (RefugeeGroupValidationException refugeeGroupValidationException)
            {
                return BadRequest(refugeeGroupValidationException.InnerException);
            }
            catch (RefugeeGroupDependencyValidationException refugeeGroupDependencyValidationException)
                when (refugeeGroupDependencyValidationException.InnerException is AlreadyExistsRefugeeGroupException)
            {
                return Conflict(refugeeGroupDependencyValidationException.InnerException);
            }
            catch (RefugeeGroupDependencyValidationException refugeeGroupDependencyValidationException)
            {
                return BadRequest(refugeeGroupDependencyValidationException.InnerException);
            }
            catch (RefugeeGroupDependencyException refugeeGroupDependencyException)
            {
                return InternalServerError(refugeeGroupDependencyException.InnerException.Message);
            }
            catch (RefugeeGroupServiceException refugeeGroupServiceException)
            {
                return InternalServerError(refugeeGroupServiceException);
            }
        }
    }
}
