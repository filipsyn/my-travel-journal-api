using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MyTravelJournal.Api.Contracts.V1;
using MyTravelJournal.Api.Contracts.V1.Requests;
using MyTravelJournal.Api.Contracts.V1.Responses;
using MyTravelJournal.Api.Services.TripService;

namespace MyTravelJournal.Api.Controllers.V1;

[ApiController]
[Route(ApiRoutes.Trip.ControllerUrl)]
[Produces("application/json")]
public class TripsController : ControllerBase
{
    private readonly ITripService _tripService;

    public TripsController(ITripService tripService)
    {
        _tripService = tripService;
    }

    [HttpGet(ApiRoutes.Trip.GetAll)]
    public async Task<ActionResult<ServiceResponse<IEnumerable<TripDetailsResponse>>>> GetAll()
    {
       return Ok(await _tripService.GetAllAsync());
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
    public async Task<ActionResult<ServiceResponse<TripDetailsResponse>>> Delete(int id)
    {
        var response = await _tripService.DeleteAsync(id);

        return StatusCode(response.Status.Code, response);
    }
}