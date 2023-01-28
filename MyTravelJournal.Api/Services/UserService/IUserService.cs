using MyTravelJournal.Api.Contracts.V1.Responses;

namespace MyTravelJournal.Api.Services.UserService;

public interface IUserService
{
    public Task<ServiceResponse<UserDetailsResponse>> GetByIdAsync(int id);
    public Task<ServiceResponse<IEnumerable<UserDetailsResponse>>> GetAllAsync();
    //public Task<ServiceResponse<UserDetailsResponse>> CreateAsync();
    //public Task<ServiceResponse<UserDetailsResponse>> UpdateAsync(int id);
    //public Task<ServiceResponse<UserDetailsResponse>> DeleteByIdAsync(int id);
} 