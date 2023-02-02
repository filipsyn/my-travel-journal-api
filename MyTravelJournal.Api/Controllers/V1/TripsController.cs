using Microsoft.AspNetCore.Mvc;
using MyTravelJournal.Api.Contracts.V1;

namespace MyTravelJournal.Api.Controllers.V1;

[ApiController]
[Route(ApiRoutes.Trip.ControllerUrl)]
[Produces("application/json")]
public class TripsController : ControllerBase
{
}