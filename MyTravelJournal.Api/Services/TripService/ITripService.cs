using Microsoft.AspNetCore.JsonPatch;
using MyTravelJournal.Api.Contracts.V1.Requests;
using MyTravelJournal.Api.Contracts.V1.Responses;

namespace MyTravelJournal.Api.Services.TripService;

public interface ITripService
{
    public Task<ServiceResponse<IEnumerable<TripDetailsResponse>>> GetAllAsync();

    public Task<ServiceResponse<TripDetailsResponse>> GetByIdAsync(int id);

    public Task<ServiceResponse<TripDetailsResponse>> CreateAsync(CreateTripRequest request);

    public Task<ServiceResponse<TripDetailsResponse>>
        UpdateAsync(JsonPatchDocument<UpdateTripRequest> request, int id);
}