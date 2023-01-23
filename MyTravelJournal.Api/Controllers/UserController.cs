using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTravelJournal.Api.Data;
using MyTravelJournal.Api.Models;
using MyTravelJournal.Api.Utils;

namespace MyTravelJournal.Api.Controllers;

[ApiController]
[Route(Endpoints.User.ControllerUrl)]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly MyTravelJournalDbContext _db;

    public UserController(MyTravelJournalDbContext db)
    {
        _db = db;
    }

    [HttpGet(Endpoints.User.GetAllUsers)]
    public async Task<ActionResult<List<User>>> GetAllUsers()
    {
        var users = _db.Users;
        if (users == null)
            return NoContent();

        var result = await users.ToListAsync();
        return Ok(result);
    }

    [HttpGet(Endpoints.User.GetUserById)]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        var users = _db.Users;
        if (users is null) return NoContent();

        var result = await users.FirstOrDefaultAsync(u => u.UserId == id);
        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost(Endpoints.User.CreateUser)]
    public async Task<ActionResult<User>> CreateUser([FromBody] User newUser)
    {
        var users = _db.Users;
        if (users is null)
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);

        users.Add(newUser);
        await _db.SaveChangesAsync();

        return Ok(newUser);
    }

    [HttpDelete(Endpoints.User.DeleteUser)]
    public ActionResult DeleteUser(int id)
    {
        return Ok("Deleted");
    }

    [HttpPatch(Endpoints.User.UpdateUser)]
    public ActionResult UpdateUser(int id)
    {
        return Ok("Updated");
    }
}