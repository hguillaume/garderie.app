using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace garderie.app2.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly SignInManager<IdentityUser> _signInManager;

    public AuthenticationController(SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
    }

    [HttpPost]
    [Route("Logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    [HttpGet]
    [Route("IsUserAuthenticated")]
    public string IsUserAuthenticated()
    {
        return (User?.Identity?.IsAuthenticated ?? false).ToString();
    }

    [HttpGet]
    [Route("GetUserName")]
    public string? GetUserName()
    {
        return User?.Identity?.Name;
    }

    [HttpGet]
    [Route("GetUserId")]
    public string? GetUserId()
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return userId;
    }
}
