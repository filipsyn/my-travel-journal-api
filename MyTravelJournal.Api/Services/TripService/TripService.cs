using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using MyTravelJournal.Api.Contracts.V1.Requests;
using MyTravelJournal.Api.Contracts.V1.Responses;
using MyTravelJournal.Api.Models;
using MyTravelJournal.Api.Repositories.TripRepository;
using ErrorOr;

namespace MyTravelJournal.Api.Services.TripService;

public class TripService : ITripService
{
    private readonly IMapper _mapper;
    private readonly ITripRepository _tripRepository;

    public TripService(IMapper mapper, ITripRepository tripRepository)
    {
        _mapper = mapper;
        _tripRepository = tripRepository;
    }

    public async Task<IEnumerable<TripDetailsResponse>> GetAllAsync()
    {
        var trips = await _tripRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<TripDetailsResponse>>(trips);
    }

    public async Task<ServiceResponse<TripDetailsResponse>> GetByIdAsync(int id)
    {
        var trip = await _tripRepository.GetByIdAsync(id);

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

        try
        {
            await _tripRepository.CreateAsync(trip);
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
        var trip = await _tripRepository.GetByIdAsync(id);

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
            await _tripRepository.UpdateAsync(trip);
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
        try
        {
            await _tripRepository.DeleteAsync(id);
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

    public async Task<ServiceResponse<List<TripDetailsResponse>>> GetTripsByUser(int userId)
    {
        //var trips = await _db.Trips.Where(t => t.UserId == userId).ToListAsync();
        var trips = await _tripRepository.GetWhereAsync(t => t.UserId == userId);

        return new ServiceResponse<List<TripDetailsResponse>>(
            StatusCodes.Status200OK,
            "Ok",
            _mapper.Map<List<TripDetailsResponse>>(trips)
        );
    }
}