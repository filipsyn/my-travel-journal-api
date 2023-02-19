using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MyTravelJournal.Api.Contracts.V1.Requests;
using MyTravelJournal.Api.Contracts.V1.Responses;
using MyTravelJournal.Api.Repositories.UserRepository;
using MyTravelJournal.Api.Services.UserService;
using ErrorOr;
using MyTravelJournal.Api.Models;

namespace MyTravelJournal.Api.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public AuthService(IUserService userService, IConfiguration configuration, IUserRepository userRepository)
    {
        _userService = userService;
        _configuration = configuration;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<Created>> RegisterAsync(CreateUserRequest request)
    {
        var foundUser = await _userRepository.GetByUsernameAsync(request.Username);

        if (foundUser is null)
            return Error.Conflict(description: "User with this username already exists");

        return Result.Created;
    }

    public async Task<ErrorOr<string>> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);

        if (user is null)
            return Error.Validation(description: "Incorrect credentials.");

        if (!await VerifyPasswordAsync(request.Username, request.Password))
            return Error.Validation(description: "Incorrect credentials.");

        return GenerateJwtToken(user);
    }

    private async Task<bool> VerifyPasswordAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        return user is not null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}