using Microsoft.AspNetCore.JsonPatch;
using MyTravelJournal.Api.Contracts.V1.Requests;
using MyTravelJournal.Api.Contracts.V1.Responses;
using ErrorOr;

namespace MyTravelJournal.Api.Services.TripService;

public interface ITripService
{
    public Task<IEnumerable<TripDetailsResponse>> GetAllAsync();

    public Task<ErrorOr<TripDetailsResponse>> GetByIdAsync(int id);

    public Task<ErrorOr<Created>> CreateAsync(CreateTripRequest request);

    public Task<ErrorOr<Updated>>
        UpdateAsync(JsonPatchDocument<UpdateTripRequest> request, int id);

    public Task<ErrorOr<Deleted>> DeleteAsync(int id);

    public Task<IEnumerable<TripDetailsResponse>> GetTripsByUser(int userId);
}