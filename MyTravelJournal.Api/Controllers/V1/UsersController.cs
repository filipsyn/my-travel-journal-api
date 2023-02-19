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
    /// <response code="200">All users successfully retrieved</response>
    [HttpGet(ApiRoutes.User.GetAllUsers)]
    //[Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ServiceResponse<IEnumerable<UserDetailsResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<UserDetailsResponse>>> GetAll()
    {
        var response = await _userService.GetAllAsync();

        //return StatusCode(response.Status.Code, response);
        return Ok(response);
    }

    /// <summary>
    /// Retrieves specific user
    /// </summary>
    /// <param name="id">An ID of searched user</param>
    /// <response code="200">Returns information about specific user</response>
    /// <response code="404">User with this ID was not found</response>
    [HttpGet(ApiRoutes.User.GetUserById)]
    [ProducesResponseType(typeof(ServiceResponse<UserDetailsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ServiceResponse<>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDetailsResponse>> GetById(int id)
    {
        var response = await _userService.GetByIdAsync(id);

        //return StatusCode(response.Status.Code, response);
        return response.Match(
            Ok,
            _ => Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "User with this ID was not found"
            )
        );
    }

    /// <summary>
    /// Deletes specific user
    /// </summary>
    /// <param name="id">ID of user to be deleted</param>
    /// <response code="200">Successfully deleted user</response>
    /// <response code="404">User with this ID wasn't found</response>
    /// <response code="409">Raised error when sending data to database</response>
    [HttpDelete(ApiRoutes.User.DeleteUser)]
    [ProducesResponseType(typeof(ServiceResponse<>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ServiceResponse<>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ServiceResponse<>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _userService.DeleteByIdAsync(id);

        return response.Match(
            result => Ok(result),
            Problem
        );
    }

    /// <summary>Performs partial update of specific user's data.</summary>
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
    /// <response code="200">Successfully updated</response>
    /// <response code="400">Invalid model state</response>
    /// <response code="404">Updating not existing user</response>
    /// <response code="409">Raised error when sending data to database</response>
    /// <response code="500">Mistake when mapping User to DTO</response>
    /// 
    [HttpPatch(ApiRoutes.User.UpdateUser)]
    [ProducesResponseType(typeof(ServiceResponse<>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ServiceResponse<>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ServiceResponse<>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ServiceResponse<>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ServiceResponse<>), StatusCodes.Status500InternalServerError)]
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

    [HttpGet(ApiRoutes.User.GetTripsForUser)]
    public async Task<ActionResult<ServiceResponse<List<TripDetailsResponse>>>> GetTripsFor(int id)
    {
        var response = await _userService.GetTripsForUser(id);

        return StatusCode(response.Status.Code, response);
    }
}