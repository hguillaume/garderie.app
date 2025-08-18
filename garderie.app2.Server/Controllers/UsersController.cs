using garderie.app2.Server.Data;
using garderie.app2.Server.Models;
using garderie.app2.Server.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace garderie.app2.Server.Controllers
{
    // localhost:port/api/Users
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public UsersController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbContext.Database.EnsureCreated();
        }

        [HttpGet(Name = "GetUsers")]
        public IActionResult Get()
        {
            var allUsers = dbContext.Users.ToList();
            return Ok(allUsers);
        }

        [HttpPost]
        public IActionResult Add([FromBody] AddUserDto userDto)
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
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var user = dbContext.Users.Find(id);

            if(HttpContext != null && HttpContext.Session != null)
                HttpContext.Session.SetString("userId", id.ToString());

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(int id, [FromBody] AddUserDto userDto)
        {
            var user = dbContext.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            user.name = userDto.name;
            user.email = userDto.email;
            user.password = userDto.password;

            // Try to validate the model
            ValidationContext vc = new ValidationContext(user); // The simplest form of validation context. It contains only a reference to the object being validated.
            ICollection<ValidationResult> results = new List<ValidationResult>(); // Will contain the results of the validation
            bool isValid = Validator.TryValidateObject(user, vc, results, true); // Validates the object and its properties using the previously created context.
            if (!isValid)
            {
                return BadRequest(results);
            }

            dbContext.SaveChanges();
            return Ok(user);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
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
