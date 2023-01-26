using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTravelJournal.Api.Data;
using MyTravelJournal.Api.DTOs;
using MyTravelJournal.Api.Models;
using MyTravelJournal.Api.Utils;

namespace MyTravelJournal.Api.Controllers;

[ApiController]
[Route(Endpoints.User.ControllerUrl)]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly MyTravelJournalDbContext _db;
    private readonly IMapper _mapper;

    public UserController(MyTravelJournalDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves list of all users.
    /// </summary>
    /// <returns></returns>
    /// <response code="200">All users retrieved</response>
    [HttpGet(Endpoints.User.GetAllUsers)]
    [ProducesResponseType(typeof(List<UserDetailsResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<UserDetailsResponse>>> GetAllUsers()
    {
        var users = await _db.Users.ToListAsync();

        return Ok(_mapper.Map<List<UserDetailsResponse>>(users));
    }

    /// <summary>
    /// Retrieves specific user
    /// </summary>
    /// <param name="id">ID of searched user</param>
    /// <returns></returns>
    /// <response code="200">Returns information about specific user</response>
    /// <response code="404">User with this ID was not found</response>
    [HttpGet(Endpoints.User.GetUserById)]
    [ProducesResponseType(typeof(UserDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDetailsResponse>> GetUserById(int id)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == id);

        if (user is null) return NotFound();

        return Ok(_mapper.Map<UserDetailsResponse>(user));
    }

    /// <summary>
    /// Creates new user
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200">New user was successfully added</response>
    /// <response code="409">Raised error when sending data to database</response>
    [HttpPost(Endpoints.User.CreateUser)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var user = _mapper.Map<User>(request);
        _db.Users.Add(user);

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return StatusCode(StatusCodes.Status409Conflict);
        }

        return Ok();
    }

    /// <summary>
    /// Deletes specific user
    /// </summary>
    /// <param name="id">ID of user to be deleted</param>
    /// <returns></returns>
    /// <response code="200">Successfully deleted user</response>
    /// <response code="404">User with this ID wasn't found</response>
    /// <response code="409">Raised error when sending data to database</response>
    [HttpDelete(Endpoints.User.DeleteUser)]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<int>> DeleteUser(int id)
    {
        var result = await _db.Users.FirstOrDefaultAsync(u => u.UserId == id);
        if (result is null)
            return NotFound();

        _db.Users.Remove(result);

        try
        {
            await _db.SaveChangesAsync();
        }

        catch (DbUpdateConcurrencyException)
        {
            return StatusCode(StatusCodes.Status409Conflict);
        }

        return Ok(id);
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
    [HttpPatch(Endpoints.User.UpdateUser)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<string>> UpdateUser([FromBody] JsonPatchDocument<UserDetailsDto> patch, int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == id);
        if (user is null)
            return NotFound();

        var userDto = _mapper.Map<UserDetailsDto>(user);
        if (userDto is null)
            return StatusCode(StatusCodes.Status500InternalServerError,
                "An error occurred while mapping the user object to the DTO.");

        patch.ApplyTo(userDto);

        _mapper.Map(userDto, user);

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return StatusCode(StatusCodes.Status409Conflict);
        }

        return NoContent();
    }
}