using Microsoft.AspNetCore.Mvc;
using MyTravelJournal.Api.Contracts.V1;

namespace MyTravelJournal.Api.Controllers.V1;

[ApiController]
[Route(ApiRoutes.Auth.ControllerUrl)]
public class AuthController : ControllerBase
{
    [HttpPost(ApiRoutes.Auth.Register)]
    public async Task<ActionResult> Register()
    {
        return Ok();
    }

    [HttpPost(ApiRoutes.Auth.Login)]
    public async Task<ActionResult> Login()
    {
        return Ok();
    }
}