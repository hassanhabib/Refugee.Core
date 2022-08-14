// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using System;
using System.Linq;
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

        [HttpGet]
        public ActionResult<IQueryable<Host>> GetAllHosts()
        {
            try
            {
                IQueryable<Host> retrievedHosts =
                    this.hostService.RetrieveAllHosts();

                return Ok(retrievedHosts);
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

        [HttpGet("{hostId}")]
        public async ValueTask<ActionResult<Host>> GetHostByIdAsync(Guid hostId)
        {
            try
            {
                Host host = await this.hostService.RetrieveHostByIdAsync(hostId);

                return Ok(host);
            }
            catch (HostValidationException hostValidationException)
                when (hostValidationException.InnerException is NotFoundHostException)
            {
                return NotFound(hostValidationException.InnerException);
            }
            catch (HostValidationException hostValidationException)
            {
                return BadRequest(hostValidationException.InnerException);
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

        [HttpPut]
        public async ValueTask<ActionResult<Host>> PutHostAsync(Host host)
        {
            try
            {
                Host modifiedHost =
                    await this.hostService.ModifyHostAsync(host);

                return Ok(modifiedHost);
            }
            catch (HostValidationException hostValidationException)
                when (hostValidationException.InnerException is NotFoundHostException)
            {
                return NotFound(hostValidationException.InnerException);
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

        [HttpDelete("{hostId}")]
        public async ValueTask<ActionResult<Host>> DeleteHostByIdAsync(Guid hostId)
        {
            try
            {
                Host deletedHost =
                    await this.hostService.RemoveHostByIdAsync(hostId);

                return Ok(deletedHost);
            }
            catch (HostValidationException hostValidationException)
                when (hostValidationException.InnerException is NotFoundHostException)
            {
                return NotFound(hostValidationException.InnerException);
            }
            catch (HostValidationException hostValidationException)
            {
                return BadRequest(hostValidationException.InnerException);
            }
            catch (HostDependencyValidationException hostDependencyValidationException)
                when (hostDependencyValidationException.InnerException is LockedHostException)
            {
                return Locked(hostDependencyValidationException.InnerException);
            }
            catch (HostDependencyValidationException hostDependencyValidationException)
            {
                return BadRequest(hostDependencyValidationException);
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