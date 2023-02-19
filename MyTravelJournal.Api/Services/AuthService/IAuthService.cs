using MyTravelJournal.Api.Contracts.V1.Requests;
using MyTravelJournal.Api.Contracts.V1.Responses;
using ErrorOr;

namespace MyTravelJournal.Api.Services.AuthService;

public interface IAuthService
{
    public Task<ErrorOr<Created>> RegisterAsync(CreateUserRequest request);

    public Task<ErrorOr<string>> LoginAsync(LoginRequest request);
}