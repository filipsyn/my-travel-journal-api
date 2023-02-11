using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyTravelJournal.Api.Contracts.V1.Requests;
using MyTravelJournal.Api.Contracts.V1.Responses;
using MyTravelJournal.Api.Data;
using MyTravelJournal.Api.Services.UserService;

namespace MyTravelJournal.Api.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly DataContext _db;
    private readonly IConfiguration _configuration;

    public AuthService(IUserService userService, DataContext db, IConfiguration configuration)
    {
        _userService = userService;
        _db = db;
        _configuration = configuration;
    }

    public async Task<ServiceResponse<string>> RegisterAsync(CreateUserRequest request)
    {
        var foundUser = await _userService.GetByUsernameAsync(request.Username);
        if (foundUser.Success)
        {
            return new ServiceResponse<string>(
                StatusCodes.Status400BadRequest,
                "User with this username already exists.",
                false
            );
        }

        return await _userService.CreateAsync(request);
    }

    public async Task<ServiceResponse<string>> LoginAsync(LoginRequest request)
    {
        var user = await _userService.GetByUsernameAsync(request.Username);
        if (!user.Success)
        {
            return new ServiceResponse<string>(
                StatusCodes.Status404NotFound,
                "User with this username doesn't exist."
            );
        }

        // Authenticate password
        if (!await VerifyPasswordAsync(request.Username, request.Password))
        {
            return new ServiceResponse<string>(
                StatusCodes.Status400BadRequest,
                "Incorrect password"
            );
        }

        // Generate JWT token
        var foundUser = await _userService.GetByUsernameAsync(user.Data!.Username);
        var token = GenerateJwtToken(foundUser.Data!);

        return new ServiceResponse<string>(
            StatusCodes.Status200OK,
            "User successfully logged in.",
            token
        );
    }

    private async Task<bool> VerifyPasswordAsync(string username, string password)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
        return user is not null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }

    private string GenerateJwtToken(UserDetailsResponse user)
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