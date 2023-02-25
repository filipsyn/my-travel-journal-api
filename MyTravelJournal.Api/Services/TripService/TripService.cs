using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using MyTravelJournal.Api.Contracts.V1.Requests;
using MyTravelJournal.Api.Contracts.V1.Responses;
using MyTravelJournal.Api.Exceptions;
using MyTravelJournal.Api.Models;
using MyTravelJournal.Api.Repositories.TripRepository;

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

    public async Task<TripDetailsResponse> GetByIdAsync(int id)
    {
        var trip = await _tripRepository.GetByIdAsync(id);

        if (trip is null)
            throw new NotFoundException("Trip was not found");

        return _mapper.Map<TripDetailsResponse>(trip);
    }

    public async Task CreateAsync(CreateTripRequest request)
    {
        var trip = _mapper.Map<Trip>(request);

        try
        {
            await _tripRepository.CreateAsync(trip);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConflictException("Conflict occured while adding to database");
        }
    }

    public async Task UpdateAsync(JsonPatchDocument<UpdateTripRequest> request,
        int id)
    {
        var trip = await _tripRepository.GetByIdAsync(id);

        if (trip is null)
            throw new NotFoundException("Trip was not found");

        var patchedTrip = _mapper.Map<JsonPatchDocument<Trip>>(request);
        if (patchedTrip is null)
            throw new Exception("Error occured while mapping objects");

        patchedTrip.ApplyTo(trip);

        try
        {
            await _tripRepository.UpdateAsync(trip);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConflictException("Conflict occured while adding to database");
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            await _tripRepository.DeleteAsync(id);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConflictException("Conflict occured while adding to database");
        }
    }

    public async Task<IEnumerable<TripDetailsResponse>> GetTripsByUser(int userId)
    {
        var trips = await _tripRepository
            .GetWhereAsync(t => t.UserId == userId);

        return _mapper.Map<IEnumerable<TripDetailsResponse>>(trips);
    }
}