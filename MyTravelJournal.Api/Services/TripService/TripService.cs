using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyTravelJournal.Api.Contracts.V1.Responses;
using MyTravelJournal.Api.Data;

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
}