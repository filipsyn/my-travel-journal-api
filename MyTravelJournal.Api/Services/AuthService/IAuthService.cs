using MyTravelJournal.Api.Contracts.V1.Requests;
using MyTravelJournal.Api.Contracts.V1.Responses;

namespace MyTravelJournal.Api.Services.AuthService;

public interface IAuthService
{
    public Task<ServiceResponse<string>> RegisterAsync(CreateUserRequest request);

    public Task<ServiceResponse<string>> LoginAsync(LoginRequest request);
}