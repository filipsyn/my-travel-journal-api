using Microsoft.AspNetCore.Mvc;
using MyTravelJournal.Api.Contracts.V1;
using MyTravelJournal.Api.Contracts.V1.Requests;
using MyTravelJournal.Api.Services.AuthService;

namespace MyTravelJournal.Api.Controllers.V1;

[ApiController]
[Route(ApiRoutes.Auth.ControllerUrl)]
public class AuthController : BaseApiController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>Registers new user</summary>
    /// <param name="request">An object containing info about new user</param>
    /// <response code="204">New user successfully created</response>
    /// <response code="409">Username is taken</response>
    [HttpPost(ApiRoutes.Auth.Register)]
    [ProducesResponseType(typeof(Created), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
    {
        var response = await _authService.RegisterAsync(request);
        return response.Match(
            _ => NoContent(),
            Problem
        );
    }

    [HttpPost(ApiRoutes.Auth.Login)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        //return StatusCode(response.Status.Code, response);
        return response.Match(
            Ok,
            Problem
        );
    }
}