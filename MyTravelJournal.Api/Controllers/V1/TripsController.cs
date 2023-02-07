using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTravelJournal.Api.Contracts.V1;
using MyTravelJournal.Api.Contracts.V1.Requests;
using MyTravelJournal.Api.Contracts.V1.Responses;
using MyTravelJournal.Api.Data;
using MyTravelJournal.Api.Services.TripService;

namespace MyTravelJournal.Api.Controllers.V1;

[ApiController]
[Route(ApiRoutes.Trip.ControllerUrl)]
[Produces("application/json")]
public class TripsController : ControllerBase
{
    private readonly DataContext _db;
    private readonly ITripService _tripService;

    public TripsController(DataContext db, ITripService tripService)
    {
        _db = db;
        _tripService = tripService;
    }

    [HttpGet(ApiRoutes.Trip.GetAll)]
    public async Task<ActionResult<ServiceResponse<IEnumerable<TripDetailsResponse>>>> GetAll()
    {
        var response = await _tripService.GetAllAsync();

        return StatusCode(response.Status.Code, response);
    }


    [HttpGet(ApiRoutes.Trip.GetById)]
    public async Task<ActionResult<ServiceResponse<TripDetailsResponse>>> GetById(int id)
    {
        var response = await _tripService.GetByIdAsync(id);

        return StatusCode(response.Status.Code, response);
    }

    [HttpPost(ApiRoutes.Trip.Create)]
    public async Task<ActionResult<ServiceResponse<TripDetailsResponse>>> Create([FromBody] CreateTripRequest request)
    {
        var response = await _tripService.CreateAsync(request);

        return StatusCode(response.Status.Code, response);
    }

    [HttpPatch(ApiRoutes.Trip.Update)]
    public async Task<ActionResult<ServiceResponse<TripDetailsResponse>>> Update(
        JsonPatchDocument<UpdateTripRequest> request, int id)
    {
        var response = await _tripService.UpdateAsync(request, id);

        return StatusCode(response.Status.Code, response);
    }

    [HttpDelete(ApiRoutes.Trip.Delete)]
    public async Task<ServiceResponse<TripDetailsResponse>> Delete(int id)
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