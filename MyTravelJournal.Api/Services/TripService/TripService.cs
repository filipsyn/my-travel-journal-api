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

    public async Task<ErrorOr<TripDetailsResponse>> GetByIdAsync(int id)
    {
        var trip = await _tripRepository.GetByIdAsync(id);

        if (trip is null)
            return Error.NotFound("Trip was not found");

        return _mapper.Map<TripDetailsResponse>(trip);
    }

    public async Task<ErrorOr<Created>> CreateAsync(CreateTripRequest request)
    {
        var trip = _mapper.Map<Trip>(request);

        try
        {
            await _tripRepository.CreateAsync(trip);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Error.Conflict("Database concurrency exception");
        }

        return Result.Created;
    }

    public async Task<ErrorOr<Updated>> UpdateAsync(JsonPatchDocument<UpdateTripRequest> request,
        int id)
    {
        var trip = await _tripRepository.GetByIdAsync(id);

        if (trip is null)
            return Error.NotFound("Trip was not found");

        var patchedTrip = _mapper.Map<JsonPatchDocument<Trip>>(request);
        if (patchedTrip is null)
            return Error.Failure("Failed mapping");

        patchedTrip.ApplyTo(trip);

        try
        {
            await _tripRepository.UpdateAsync(trip);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Error.Conflict("Database concurrency exception");
        }

        return Result.Updated;
    }

    public async Task<ErrorOr<Deleted>> DeleteAsync(int id)
    {
        try
        {
            await _tripRepository.DeleteAsync(id);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Error.Conflict("Database concurrency exception");
        }

        return Result.Deleted;
    }

    public async Task<IEnumerable<TripDetailsResponse>> GetTripsByUser(int userId)
    {
        var trips = await _tripRepository
            .GetWhereAsync(t => t.UserId == userId);

        return _mapper.Map<IEnumerable<TripDetailsResponse>>(trips);
    }
}