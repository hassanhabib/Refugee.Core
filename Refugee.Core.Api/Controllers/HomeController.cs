// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO DELIVER HUMANITARIAN AID, HOPE AND LOVE
// -------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace Refugee.Core.Api.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : RESTFulController
    {
        [HttpGet]
        public ActionResult<string> Get() =>
            Ok("Sorry Mario, but our princess is in another castle.");
    }
}