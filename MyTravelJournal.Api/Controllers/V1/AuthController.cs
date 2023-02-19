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

    [HttpPost(ApiRoutes.Auth.Register)]
    public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
    {
        var response = await _authService.RegisterAsync(request);
        return response.Match(
            result => Ok(result),
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