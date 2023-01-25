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

    [HttpGet(Endpoints.User.GetAllUsers)]
    public async Task<ActionResult<List<UserDetailsDto>>> GetAllUsers()
    {
        var users = _db.Users;
        if (users == null)
            return NoContent();

        var list = await users.Select(u => new UserDetailsDto
            {
                UserId = u.UserId,
                Username = u.Username,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
            })
            .ToListAsync();

        return Ok(list);
    }

    [HttpGet(Endpoints.User.GetUserById)]
    public async Task<ActionResult<UserDetailsDto>> GetUserById(int id)
    {
        var users = _db.Users;
        if (users is null) return NoContent();

        var result = await users.Select(u => new UserDetailsDto
            {
                UserId = u.UserId,
                Username = u.Username,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
            })
            .FirstOrDefaultAsync(u => u.UserId == id);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost(Endpoints.User.CreateUser)]
    public async Task<ActionResult> CreateUser([FromBody] UserDetailsDto request)
    {
        var users = _db.Users;
        if (users is null)
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);

        users.Add(new User
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Username = request.Username
        });
        await _db.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete(Endpoints.User.DeleteUser)]
    public async Task<ActionResult<User>> DeleteUser(int id)
    {
        var users = _db.Users;
        if (users is null)
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);

        var result = await users.FirstOrDefaultAsync(u => u.UserId == id);
        if (result is null)
            return NotFound();

        users.Remove(result);
        await _db.SaveChangesAsync();

        return Ok(result);
    }

    [HttpPatch(Endpoints.User.UpdateUser)]
    public async Task<ActionResult<string>> UpdateUser([FromBody] JsonPatchDocument<UserDetailsDto> patch, int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (_db.Users is null)
            return NotFound();

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