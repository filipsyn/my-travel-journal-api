using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTravelJournal.Api.Contracts.V1;
using MyTravelJournal.Api.Contracts.V1.Responses;
using MyTravelJournal.Api.Data;

namespace MyTravelJournal.Api.Controllers.V1;

[ApiController]
[Route(ApiRoutes.Trip.ControllerUrl)]
[Produces("application/json")]
public class TripsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly DataContext _db;

    public TripsController(IMapper mapper, DataContext db)
    {
        _mapper = mapper;
        _db = db;
    }

    [HttpGet(ApiRoutes.Trip.GetAll)]
    public async Task<ServiceResponse<IEnumerable<TripDetailsResponse>>> GetAll()
    {
        var trips = await _db.Trips.ToListAsync();
        
        return new ServiceResponse<IEnumerable<TripDetailsResponse>>(
            StatusCodes.Status200OK,
            "List of trips successfully retrieved.",
            _mapper.Map<IEnumerable<TripDetailsResponse>>(trips)
        );
    }
}