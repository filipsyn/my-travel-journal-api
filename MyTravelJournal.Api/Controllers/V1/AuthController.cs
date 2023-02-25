using Microsoft.AspNetCore.Mvc;
using MyTravelJournal.Api.Contracts.V1;
using MyTravelJournal.Api.Contracts.V1.Requests;
using MyTravelJournal.Api.Services.AuthService;

namespace MyTravelJournal.Api.Controllers.V1;

[ApiController]
[Route(ApiRoutes.Auth.ControllerUrl)]
public class AuthController : ControllerBase
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
    {
        await _authService.RegisterAsync(request);

        return NoContent();
    }

    /// <summary>
    /// Generates JWT token for user's login session
    /// </summary>
    /// <param name="request">An object containing user's credentials</param>
    /// 
    /// <response code="200">User successfully logged in</response>
    /// <response code="400">Incorrect credentials</response>
    [HttpPost(ApiRoutes.Auth.Login)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);

        return Ok(response);
    }
}