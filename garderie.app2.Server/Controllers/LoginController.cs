// TODO: Login (Post)
// TODO: GetLoggedInUser (Get)
// TODO: Logout (Delete)


using garderie.app2.Server.Data;
using garderie.app2.Server.Models;
using garderie.app2.Server.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace garderie.app2.Server.Controllers
{
    // localhost:port/api/login
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public LoginController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbContext.Database.EnsureCreated();
        }

        [HttpPost]
        public IActionResult Login([FromBody] AddUserDto userDto)
        {
            var user = new Models.Entities.User
            {
                name = userDto.name,
                email = userDto.email,
                password = userDto.password
            };

            // Try to validate the model
            ValidationContext vc = new ValidationContext(user); // The simplest form of validation context. It contains only a reference to the object being validated.
            ICollection<ValidationResult> results = new List<ValidationResult>(); // Will contain the results of the validation
            bool isValid = Validator.TryValidateObject(user, vc, results, true); // Validates the object and its properties using the previously created context.
            if (!isValid) {
                return BadRequest(results);
            }

            dbContext.Users.Add(user);
            int result = dbContext.SaveChanges();
            if (result < 1)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return StatusCode(StatusCodes.Status201Created, user);
        }

        [HttpGet]
        //[Route("{id}")]
        public IActionResult GetLoggedInUser()
        {
            int id = 0;
            if (HttpContext != null && HttpContext.Session != null)
                id = int.Parse(HttpContext.Session.GetString("userId"));

            var user = dbContext.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Logout(int id)
        {
            var user = dbContext.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            dbContext.Users.Remove(user);
            dbContext.SaveChanges();
            return Ok();
        }
    }
}
