using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MyTravelJournal.Api.Contracts.V1;
using MyTravelJournal.Api.Contracts.V1.Requests;
using MyTravelJournal.Api.Contracts.V1.Responses;
using MyTravelJournal.Api.Services.UserService;

namespace MyTravelJournal.Api.Controllers.V1;

[ApiController]
[Route(ApiRoutes.User.ControllerUrl)]
[Produces("application/json")]
public class UsersController : BaseApiController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Retrieves list of all users.
    /// </summary>
    /// <response code="200">Users successfully retrieved</response>
    [HttpGet(ApiRoutes.User.GetAllUsers)]
    [ProducesResponseType(typeof(IEnumerable<UserDetailsResponse>), StatusCodes.Status200OK)]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var response = await _userService.GetAllAsync();

        return Ok(response);
    }

    /// <summary>
    /// Retrieves specific user
    /// </summary>
    /// <param name="id">An ID of searched user</param>
    /// <response code="200">Returns information about specific user</response>
    /// <response code="404">User was not found</response>
    [HttpGet(ApiRoutes.User.GetUserById)]
    [ProducesResponseType(typeof(UserDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _userService.GetByIdAsync(id);

        return response.Match(
            Ok,
            Problem
        );
    }

    /// <summary>
    /// Deletes specific user
    /// </summary>
    /// <param name="id">ID of user to be deleted</param>
    /// <response code="204">Successfully deleted</response>
    /// <response code="404">User was not found</response>
    /// <response code="409">Error on writing to the database</response>
    [HttpDelete(ApiRoutes.User.DeleteUser)]
    [ProducesResponseType(typeof(Deleted), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _userService.DeleteByIdAsync(id);

        return response.Match(
            _ => NoContent(),
            Problem
        );
    }

    /// <summary>Patches specific user's data.</summary>
    /// <param name="patch">A request body</param>
    /// <param name="id">An ID of updated user</param>
    /// <remarks>
    ///     Request has a structure of standard JSON patch request.
    /// 
    ///     Following example shows request to change user's username:
    ///     
    ///     ```
    ///     [
    ///         {
    ///             "path" : "/username",
    ///             "op" : "replace",
    ///             "value" : "new.username"
    ///         }
    ///     ]
    ///     ```
    /// </remarks>
    /// <response code="204">Successfully updated</response>
    /// <response code="400">Invalid model state</response>
    /// <response code="404">User does not exist</response>
    /// <response code="409">Error on writing to the database</response>
    /// <response code="500">Error on mapping User to DTO</response>
    /// 
    [HttpPatch(ApiRoutes.User.UpdateUser)]
    [ProducesResponseType(typeof(Updated), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUser([FromBody] JsonPatchDocument<UpdateUserDetailsRequest> patch,
        int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _userService.UpdateAsync(patch, id);

        return response.Match(
            _ => NoContent(),
            Problem
        );
    }

    /// <summary>
    /// Retrieves list of trips for selected user
    /// </summary>
    /// <param name="id">An ID of user</param>
    /// <response code="200">List successfully retrieved</response>
    /// <response code="404">User not found</response>
    [HttpGet(ApiRoutes.User.GetTripsForUser)]
    [ProducesResponseType(typeof(IEnumerable<TripDetailsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTripsFor(int id)
    {
        var response = await _userService.GetTripsForUser(id);

        return response.Match(
            Ok,
            Problem
        );
    }
}