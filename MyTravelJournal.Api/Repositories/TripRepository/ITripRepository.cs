using System.Linq.Expressions;
using MyTravelJournal.Api.Models;

namespace MyTravelJournal.Api.Repositories.TripRepository;

public interface ITripRepository
{
    public Task<Trip?> GetByIdAsync(int tripId);

    public Task<IEnumerable<Trip>> GetAllAsync();

    public Task<IEnumerable<Trip>> GetWhereAsync(Expression<Func<Trip, bool>> predicate);

    public Task CreateAsync(Trip trip);

    public Task UpdateAsync(Trip trip);

    public Task DeleteAsync(int tripId);
}