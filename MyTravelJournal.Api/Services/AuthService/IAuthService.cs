using MyTravelJournal.Api.Contracts.V1.Requests;
using MyTravelJournal.Api.Contracts.V1.Responses;

namespace MyTravelJournal.Api.Services.AuthService;

public interface IAuthService
{
    // Register Async
    public Task<ServiceResponse<string>> RegisterAsync(CreateUserRequest request);

    // Login Async
    public Task<ServiceResponse<string>> LoginAsync(LoginRequest request);

    // Generate Token

    // Authenticate Password

    // Generate Refresh Token
}