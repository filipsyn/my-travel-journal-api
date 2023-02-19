using ErrorOr;
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

    [HttpGet(ApiRoutes.Trip.GetAll)]
    public async Task<ActionResult<ServiceResponse<IEnumerable<TripDetailsResponse>>>> GetAll()
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
    public async Task<ActionResult<ServiceResponse<TripDetailsResponse>>> Delete(int id)
    {
        var response = await _tripService.DeleteAsync(id);

        return StatusCode(response.Status.Code, response);
    }
}