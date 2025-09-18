using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.WebUtilities;

namespace garderie.app2.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class LogoutController : ControllerBase
{
    private readonly SignInManager<IdentityUser> _signInManager;

    public LogoutController(SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
    }

    [HttpPost]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Redirect("/");
    }
}
