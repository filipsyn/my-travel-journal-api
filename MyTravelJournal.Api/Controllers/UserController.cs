using Microsoft.AspNetCore.Mvc;
using MyTravelJournal.Api.Utils;

namespace MyTravelJournal.Api.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    [HttpGet]
    [Route(Endpoints.User.GetAllUsers)]
    public List<string> GetAllUsers()
    {
        return new List<string>() { "User", "John" };
    }

    [HttpGet]
    [Route(Endpoints.User.GetUser)]
    public string GetUser()
    {
        return "Sample user";
    }

    [HttpPost]
    [Route(Endpoints.User.CreateUser)]
    public void CreateUser()
    {
    }

    [HttpDelete]
    [Route(Endpoints.User.DeleteUser)]
    public void DeleteUser(int id)
    {
    }

    [HttpPatch]
    [Route(Endpoints.User.UpdateUser)]
    public void UpdateUser(int id)
    {
    }
}