using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using MyTravelJournal.Api.Contracts.V1.Requests;
using MyTravelJournal.Api.Contracts.V1.Responses;
using MyTravelJournal.Api.Data;
using MyTravelJournal.Api.Models;

namespace MyTravelJournal.Api.Services.TripService;

public class TripService : ITripService
{
    private readonly DataContext _db;
    private readonly IMapper _mapper;

    public TripService(DataContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<IEnumerable<TripDetailsResponse>>> GetAllAsync()
    {
        var trips = await _db.Trips.ToListAsync();

        return new ServiceResponse<IEnumerable<TripDetailsResponse>>(
            StatusCodes.Status200OK,
            "List of trips successfully retrieved.",
            _mapper.Map<IEnumerable<TripDetailsResponse>>(trips)
        );
    }

    public async Task<ServiceResponse<TripDetailsResponse>> GetByIdAsync(int id)
    {
        var trip = await _db.Trips.FirstOrDefaultAsync(t => t.TripId == id);

        if (trip is null)
        {
            return new ServiceResponse<TripDetailsResponse>(
                StatusCodes.Status404NotFound,
                "Trip with this ID was not found."
            );
        }

        return new ServiceResponse<TripDetailsResponse>(
            StatusCodes.Status200OK,
            "Trip with this ID was found.",
            _mapper.Map<TripDetailsResponse>(trip)
        );
    }

    public async Task<ServiceResponse<TripDetailsResponse>> CreateAsync(CreateTripRequest request)
    {
        var trip = _mapper.Map<Trip>(request);

        _db.Trips.Add(trip);

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return new ServiceResponse<TripDetailsResponse>(
                StatusCodes.Status409Conflict,
                ex.ToString()
            );
        }

        return new ServiceResponse<TripDetailsResponse>(
            StatusCodes.Status200OK,
            "Trip was successfully added."
        );
    }

    public async Task<ServiceResponse<TripDetailsResponse>> UpdateAsync(JsonPatchDocument<UpdateTripRequest> request,
        int id)
    {
        var trip = await _db.Trips.FirstOrDefaultAsync(t => t.TripId == id);
        if (trip is null)
        {
            return new ServiceResponse<TripDetailsResponse>(StatusCodes.Status404NotFound,
                "Trip with this ID was not found.");
        }

        var patchedTrip = _mapper.Map<JsonPatchDocument<Trip>>(request);
        if (patchedTrip is null)
        {
            return new ServiceResponse<TripDetailsResponse>(
                StatusCodes.Status500InternalServerError,
                "Mapping of objects was unsuccessfully."
            );
        }

        patchedTrip.ApplyTo(trip);

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return new ServiceResponse<TripDetailsResponse>(
                StatusCodes.Status409Conflict,
                ex.ToString()
            );
        }

        return new ServiceResponse<TripDetailsResponse>(
            StatusCodes.Status200OK,
            "Trip was successfully updated."
        );
    }

    public async Task<ServiceResponse<TripDetailsResponse>> DeleteAsync(int id)
    {
        var trip = await _db.Trips.FirstOrDefaultAsync(t => t.TripId == id);
        if (trip is null)
        {
            return new ServiceResponse<TripDetailsResponse>(
                StatusCodes.Status404NotFound,
                "Trip with this ID was not found"
            );
        }

        _db.Trips.Remove(trip);

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return new ServiceResponse<TripDetailsResponse>(
                StatusCodes.Status409Conflict,
                ex.ToString()
            );
        }

        return new ServiceResponse<TripDetailsResponse>(
            StatusCodes.Status200OK,
            "Trip was successfully deleted."
        );
    }
}