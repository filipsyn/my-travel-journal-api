using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MyTravelJournal.Api.Contracts.V1.Requests;
using MyTravelJournal.Api.Repositories.UserRepository;
using MyTravelJournal.Api.Services.UserService;
using MyTravelJournal.Api.Exceptions;
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

    public async Task RegisterAsync(CreateUserRequest request)
    {
        var foundUser = await _userRepository.GetByUsernameAsync(request.Username);

        if (foundUser is not null)
            throw new NotFoundException("User was not found");

        await _userService.CreateAsync(request);
    }

    public async Task<string> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);

        if (user is null)
            throw new ValidationException("Incorrect credentials");

        if (!await VerifyPasswordAsync(request.Username, request.Password))
            throw new ValidationException("Incorrect credentials");

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