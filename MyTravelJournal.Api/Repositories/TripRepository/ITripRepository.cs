using MyTravelJournal.Api.Models;

namespace MyTravelJournal.Api.Repositories.TripRepository;

public interface ITripRepository
{
    
   public Task<Trip?> GetByIdAsync(int tripId);

   public Task<IEnumerable<Trip>> GetAllAsync();

   public Task CreateAsync(Trip trip);

   public Task UpdateAsync(Trip trip);

   public Task DeleteAsync(int tripId);
}