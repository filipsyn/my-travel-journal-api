using ErrorOr;
using Microsoft.AspNetCore.JsonPatch;
using MyTravelJournal.Api.Contracts.V1.Requests;
using MyTravelJournal.Api.Contracts.V1.Responses;

namespace MyTravelJournal.Api.Services.UserService;

public interface IUserService
{
    public Task<UserDetailsResponse> GetByIdAsync(int id);


    public Task<IEnumerable<UserDetailsResponse>> GetAllAsync();


    public Task<UserDetailsResponse> GetByUsernameAsync(string username);


    public Task CreateAsync(CreateUserRequest request);


    public Task UpdateAsync(JsonPatchDocument<UpdateUserDetailsRequest> patchRequest, int id);


    public Task DeleteByIdAsync(int id);

    public Task<IEnumerable<TripDetailsResponse>> GetTripsForUser(int id);
}