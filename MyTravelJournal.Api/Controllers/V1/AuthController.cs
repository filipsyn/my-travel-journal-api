using Microsoft.AspNetCore.Mvc;
using MyTravelJournal.Api.Contracts.V1;
using MyTravelJournal.Api.Contracts.V1.Requests;
using MyTravelJournal.Api.Contracts.V1.Responses;
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

    [HttpPost(ApiRoutes.Auth.Register)]
    public async Task<ActionResult<ServiceResponse<string>>> Register([FromBody] CreateUserRequest request)
    {
        var response = await _authService.RegisterAsync(request);
        return StatusCode(response.Details.Code, response);
    }

    [HttpPost(ApiRoutes.Auth.Login)]
    public async Task<ActionResult<ServiceResponse<string>>> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        return StatusCode(response.Details.Code, response);
    }
}