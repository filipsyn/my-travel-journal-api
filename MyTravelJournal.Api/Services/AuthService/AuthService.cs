using AutoMapper;
using MyTravelJournal.Api.Contracts.V1.Requests;
using MyTravelJournal.Api.Contracts.V1.Responses;
using MyTravelJournal.Api.Models;
using MyTravelJournal.Api.Services.UserService;

namespace MyTravelJournal.Api.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public AuthService(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
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

        var user = _mapper.Map<User>(request);

        return await _userService.CreateAsync(request);
    }

}