using Microsoft.AspNetCore.Mvc;
using MyTravelJournal.Api.Utils;

namespace MyTravelJournal.Api.Controllers;

[ApiController]
[Route(Endpoints.User.ControllerUrl)]
public class UserController : ControllerBase
{
    [HttpGet(Endpoints.User.GetAllUsers)]
    public List<string> GetAllUsers()
    {
        return new List<string>() { "User", "John" };
    }

    [HttpGet(Endpoints.User.GetUserById)]
    public string GetUserById(int id)
    {
        return "Sample user";
    }

    [HttpPost(Endpoints.User.CreateUser)]
    public void CreateUser()
    {
    }

    [HttpDelete(Endpoints.User.DeleteUser)]
    public void DeleteUser(int id)
    {
    }

    [HttpPatch(Endpoints.User.UpdateUser)]
    public void UpdateUser(int id)
    {
    }
}