using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MyTravelJournal.Api.Data;
using MyTravelJournal.Api.Models;

namespace MyTravelJournal.Api.Repositories.TripRepository;

public class TripRepository : ITripRepository
{
    private readonly DataContext _db;

    public TripRepository(DataContext db)
    {
        _db = db;
    }

    public async Task<Trip?> GetByIdAsync(int tripId)
    {
        return await _db.Trips.FirstOrDefaultAsync(t => t.TripId == tripId);
    }

    public async Task<IEnumerable<Trip>> GetAllAsync()
    {
        return await _db.Trips.ToListAsync();
    }

    public async Task<IEnumerable<Trip>> GetWhereAsync(Expression<Func<Trip, bool>> predicate)
    {
        return await _db.Trips.Where(predicate).ToListAsync();
    }
    public async Task CreateAsync(Trip trip)
    {
        _db.Trips.Add(trip);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Trip trip)
    {
        _db.Entry(trip).State = EntityState.Modified;
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int tripId)
    {
        var trip = await this.GetByIdAsync(tripId);
        if (trip is null) return;

        _db.Trips.Remove(trip);
        await _db.SaveChangesAsync();
    }
}