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
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Retrieves list of all users.
    /// </summary>
    /// <returns></returns>
    /// <response code="200">All users retrieved</response>
    [HttpGet(ApiRoutes.User.GetAllUsers)]
    [ProducesResponseType(typeof(List<UserDetailsResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<UserDetailsResponse>>> GetAllUsers()
    {
        var response = await _userService.GetAllAsync();

        return Ok(response.Data);
    }

    /// <summary>
    /// Retrieves specific user
    /// </summary>
    /// <param name="id">ID of searched user</param>
    /// <returns></returns>
    /// <response code="200">Returns information about specific user</response>
    /// <response code="404">User with this ID was not found</response>
    [HttpGet(ApiRoutes.User.GetUserById)]
    [ProducesResponseType(typeof(UserDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDetailsResponse>> GetUserById(int id)
    {
        var response = await _userService.GetByIdAsync(id);

        return !response.Success
            ? StatusCode(response.Details.Code, response.Details)
            : Ok(response.Data);
    }

    /// <summary>
    /// Creates new user
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200">New user was successfully added</response>
    /// <response code="409">Raised error when sending data to database</response>
    [HttpPost(ApiRoutes.User.CreateUser)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        // TODO: Document response code 204
        // TODO: Change response to Created and fetch new resource URI
        var response = await _userService.CreateAsync(request);

        return !response.Success
            ? StatusCode(response.Details.Code, response.Details)
            : NoContent();
    }

    /// <summary>
    /// Deletes specific user
    /// </summary>
    /// <param name="id">ID of user to be deleted</param>
    /// <returns></returns>
    /// <response code="200">Successfully deleted user</response>
    /// <response code="404">User with this ID wasn't found</response>
    /// <response code="409">Raised error when sending data to database</response>
    [HttpDelete(ApiRoutes.User.DeleteUser)]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<int>> DeleteUser(int id)
    {
        var response = await _userService.DeleteByIdAsync(id);

        return StatusCode(response.Details.Code, response);
    }

    /// <summary>Performs partial update of specific user's data.</summary>
    /// <param name="patch">Request body</param>
    /// <param name="id">ID of updated user</param>
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
    /// <response code="404">Updating not existing user</response>
    /// <response code="409">Raised error when sending data to database</response>
    /// <response code="500">Mistake when mapping User to DTO</response>
    /// 
    [HttpPatch(ApiRoutes.User.UpdateUser)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<string>> UpdateUser([FromBody] JsonPatchDocument<UpdateUserDetailsRequest> patch,
        int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _userService.UpdateAsync(patch, id);

        return StatusCode(response.Details.Code, response);
    }
}