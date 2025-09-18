using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace garderie.app2.Server.Controllers;

[ApiController]
//[Route("[controller]")]
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
        //return Redirect("/");
        return Ok();
    }

    [HttpGet]
    [Route("IsUserAuthenticated")]
    //[Authorize]
    public string IsUserAuthenticated()
    {
        //return "Is User Authenticated: " + User.Identity.IsAuthenticated
        //    + "\n Authentication type: " + User.Identity.AuthenticationType;
        return (User.Identity.IsAuthenticated).ToString();
    }
}
