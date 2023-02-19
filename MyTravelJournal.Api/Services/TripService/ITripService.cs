using Microsoft.AspNetCore.JsonPatch;
using MyTravelJournal.Api.Contracts.V1.Requests;
using MyTravelJournal.Api.Contracts.V1.Responses;
using ErrorOr;

namespace MyTravelJournal.Api.Services.TripService;

public interface ITripService
{
    public Task<IEnumerable<TripDetailsResponse>> GetAllAsync();

    public Task<ErrorOr<TripDetailsResponse>> GetByIdAsync(int id);

    public Task<ServiceResponse<TripDetailsResponse>> CreateAsync(CreateTripRequest request);

    public Task<ServiceResponse<TripDetailsResponse>>
        UpdateAsync(JsonPatchDocument<UpdateTripRequest> request, int id);

    public Task<ServiceResponse<TripDetailsResponse>> DeleteAsync(int id);

    public Task<ServiceResponse<List<TripDetailsResponse>>> GetTripsByUser(int userId);
}