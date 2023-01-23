using Microsoft.AspNetCore.Mvc;
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
    public ActionResult<List<string>> GetAllUsers()
    {
        return Ok(new List<string>() { "User", "John" });
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