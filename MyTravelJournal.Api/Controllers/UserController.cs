using Microsoft.AspNetCore.Mvc;
using MyTravelJournal.Api.Utils;

namespace MyTravelJournal.Api.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    
    [HttpGet]
    [Route(Endpoints.User.GetUser)]
    public string GetUser()
    {
        return "Hello";
    }
}