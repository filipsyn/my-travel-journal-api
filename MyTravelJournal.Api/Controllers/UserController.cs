using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTravelJournal.Api.Data;
using MyTravelJournal.Api.Models;
using MyTravelJournal.Api.Utils;

namespace MyTravelJournal.Api.Controllers;

[ApiController]
[Route(Endpoints.User.ControllerUrl)]
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
        if (_db.Users == null) return NoContent();

        var users = await _db.Users.ToListAsync();
        return Ok(users);
    }

    [HttpGet(Endpoints.User.GetUserById)]
    public ActionResult<string> GetUserById(int id)
    {
        return Ok("Sample user");
    }

    [HttpPost(Endpoints.User.CreateUser)]
    public ActionResult CreateUser()
    {
        return Ok("Created");
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