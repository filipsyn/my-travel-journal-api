using MyTravelJournal.Api.Contracts.V1.Requests;
using MyTravelJournal.Api.Contracts.V1.Responses;

namespace MyTravelJournal.Api.Services.UserService;

public interface IUserService
{
    public Task<ServiceResponse<UserDetailsResponse>> GetByIdAsync(int id);
    public Task<ServiceResponse<IEnumerable<UserDetailsResponse>>> GetAllAsync();
    public Task<ServiceResponse<UserDetailsResponse>> CreateAsync(CreateUserRequest request);
    //public Task<ServiceResponse<UserDetailsResponse>> UpdateAsync(int id);
    //public Task<ServiceResponse<UserDetailsResponse>> DeleteByIdAsync(int id);
} 