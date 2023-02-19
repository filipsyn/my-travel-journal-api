using System.Collections;
using ErrorOr;
using Microsoft.AspNetCore.JsonPatch;
using MyTravelJournal.Api.Contracts.V1.Requests;
using MyTravelJournal.Api.Contracts.V1.Responses;

namespace MyTravelJournal.Api.Services.UserService;

public interface IUserService
{
    public Task<ErrorOr<UserDetailsResponse>> GetByIdAsync(int id);


    public Task<IEnumerable<UserDetailsResponse>> GetAllAsync();


    public Task<ErrorOr<UserDetailsResponse>> GetByUsernameAsync(string username);


    public Task<ErrorOr<Created>> CreateAsync(CreateUserRequest request);


    public Task<ErrorOr<Updated>> UpdateAsync(JsonPatchDocument<UpdateUserDetailsRequest> patchRequest, int id);


    public Task<ErrorOr<Deleted>> DeleteByIdAsync(int id);

    public Task<ErrorOr<IEnumerable<TripDetailsResponse>>> GetTripsForUser(int id);
}