using MyTravelJournal.Api.Contracts.V1.Requests;
using MyTravelJournal.Api.Contracts.V1.Responses;
using MyTravelJournal.Api.Services.UserService;

namespace MyTravelJournal.Api.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;

    public AuthService(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<ServiceResponse<string>> RegisterAsync(CreateUserRequest request)
    {
        var foundUser = await _userService.GetByUsernameAsync(request.Username);
        if (foundUser.Success)
        {
            return new ServiceResponse<string>
            {
                Success = false,
                Details = new StatusDetails
                {
                    Code = StatusCodes.Status400BadRequest,
                    Message = "User with this username already exists."
                }
            };
        }

        return await _userService.CreateAsync(request);
    }
}