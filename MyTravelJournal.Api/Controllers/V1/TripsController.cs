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
public class TripsController : BaseApiController
{
    private readonly ITripService _tripService;

    public TripsController(ITripService tripService)
    {
        _tripService = tripService;
    }

    /// <summary>
    /// Retrieves list of all trips
    /// </summary>
    /// <response code="200">List successfully retrieved</response>
    [HttpGet(ApiRoutes.Trip.GetAll)]
    [ProducesResponseType(typeof(IEnumerable<TripDetailsResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _tripService.GetAllAsync());
    }


    [HttpGet(ApiRoutes.Trip.GetById)]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _tripService.GetByIdAsync(id);

        return response.Match(
            Ok,
            Problem
        );
    }

    [HttpPost(ApiRoutes.Trip.Create)]
    public async Task<IActionResult> Create([FromBody] CreateTripRequest request)
    {
        var response = await _tripService.CreateAsync(request);

        return response.Match(
            result => Ok(result),
            Problem
        );
    }

    [HttpPatch(ApiRoutes.Trip.Update)]
    public async Task<IActionResult> Update(
        JsonPatchDocument<UpdateTripRequest> request, int id)
    {
        var response = await _tripService.UpdateAsync(request, id);

        //return StatusCode(response.Status.Code, response);
        return response.Match(
            result => Ok(result),
            Problem
        );
    }

    [HttpDelete(ApiRoutes.Trip.Delete)]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _tripService.DeleteAsync(id);

        return response.Match(
            _ => NoContent(),
            Problem
        );
    }
}