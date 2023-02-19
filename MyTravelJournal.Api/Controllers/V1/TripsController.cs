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


    /// <summary>
    /// Retrieves information about specific trip
    /// </summary>
    /// <param name="id">An ID of searched trip</param>
    /// <response code="200">Trip successfully retrieved</response>
    /// <response code="404">Trip not found</response>
    [HttpGet(ApiRoutes.Trip.GetById)]
    [ProducesResponseType(typeof(TripDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _tripService.GetByIdAsync(id);

        return response.Match(
            Ok,
            Problem
        );
    }

    /// <summary>
    /// Creates new trip
    /// </summary>
    /// <param name="request">An object containing information about created trip</param>
    /// <response code="204">Trip was successfully created</response>
    /// <response code="409">Error on writing to the database</response>
    [HttpPost(ApiRoutes.Trip.Create)]
    [ProducesResponseType(typeof(Created), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateTripRequest request)
    {
        var response = await _tripService.CreateAsync(request);

        return response.Match(
            _ => NoContent(),
            Problem
        );
    }

    /// <summary>
    /// Patches information about specific trip
    /// </summary>
    /// <param name="request">An object containing info about changed data</param>
    /// <param name="id">An ID of changed trip</param>
    /// <remarks>
    ///     Request has a structure of standard JSON patch request.
    /// 
    ///     Following example shows request to change title of trip:
    ///     
    ///     ```
    ///     [
    ///         {
    ///             "path" : "/title",
    ///             "op" : "replace",
    ///             "value" : "New Title!"
    ///         }
    ///     ]
    ///     ```
    /// </remarks>
    /// <response code="204">Trip successfully updated</response>
    /// <response code="404">Trip was not found</response>
    /// <response code="409">Error on writing to the database</response>
    /// <response code="500">Failed mapping of objects</response>
    [HttpPatch(ApiRoutes.Trip.Update)]
    [ProducesResponseType(typeof(Updated), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(
        JsonPatchDocument<UpdateTripRequest> request, int id)
    {
        var response = await _tripService.UpdateAsync(request, id);

        return response.Match(
            _ => NoContent(),
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