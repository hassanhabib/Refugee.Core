using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RefugeeLand.Core.Api.Models.Refugees;
using RefugeeLand.Core.Api.Services.Foundations.Refugees;
using RESTFulSense.Controllers;

namespace RefugeeLand.Core.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RefugeesController : RESTFulController
{
    private readonly IRefugeeService refugeeService;

    public RefugeesController(IRefugeeService refugeeService) =>
        this.refugeeService = refugeeService;

    [HttpPost]
    public async ValueTask<ActionResult<Refugee>> PostRefugeeAsync(Refugee refugee) =>
        throw new NotImplementedException();
}