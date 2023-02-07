using MyTravelJournal.Api.Contracts.V1.Responses;

namespace MyTravelJournal.Api.Services.TripService;

public interface ITripService
{
   public Task<ServiceResponse<IEnumerable<TripDetailsResponse>>> GetAllAsync();

   public Task<ServiceResponse<TripDetailsResponse>> GetByIdAsync(int id);
}