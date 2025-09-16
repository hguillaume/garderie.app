using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace garderie.app2.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class IsUserAuthenticatedController : ControllerBase
{
    //private static readonly string[] Summaries = new[]
    //{
    //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    //};

    //private readonly ILogger<WeatherForecastController> _logger;

    //public WeatherForecastController(ILogger<WeatherForecastController> logger)
    //{
    //    _logger = logger;
    //}

    [HttpGet]
    //[Authorize]
    public string Get()
    {
        return "Is User Authenticated: " + User.Identity.IsAuthenticated;
    }
}
