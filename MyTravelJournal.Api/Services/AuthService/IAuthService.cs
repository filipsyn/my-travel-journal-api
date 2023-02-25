using MyTravelJournal.Api.Contracts.V1.Requests;
using ErrorOr;

namespace MyTravelJournal.Api.Services.AuthService;

public interface IAuthService
{
    public Task RegisterAsync(CreateUserRequest request);

    public Task<string> LoginAsync(LoginRequest request);
}