using garderie.app2.Server.Data;
using garderie.app2.Server.Models;
using garderie.app2.Server.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Security.Claims;

namespace garderie.app2.Server.Controllers
{
    // localhost:port/api/Daycares
    [Route("api/[controller]")]
    [ApiController]
    public class DaycaresController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public DaycaresController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbContext.Database.EnsureCreated();
        }

        public string? GetUserId()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId;
        }

        [HttpGet(Name = "GetDaycares")]
        public IActionResult Get()
        {
            string? userId = GetUserId();

            Debug.WriteLine(userId);

            //var allDaycares = dbContext.Users
            //    .SelectMany(user => user.Daycares)
            //    .Where(daycare => daycare.AspNetUserId == userId)
            //    .ToList();

            var allDaycares = dbContext.Daycares
                //.SelectMany(user => user.Daycares)
                .Where(daycare => daycare.AspNetUserId == userId)
                .ToList();
            return Ok(allDaycares);
        }

        [HttpPost]
        public IActionResult Add([FromBody] AddDaycareDto daycareDto)
        {
            string? userId = GetUserId();
            var daycare = new Daycare
            {
                name = daycareDto.name,
                AspNetUserId = userId,
                //AspNetUserId = daycareDto.AspNetUserId,
                //email = userDto.email,
                //password = userDto.password
            };

            // Try to validate the model
            ValidationContext vc = new ValidationContext(daycare); // The simplest form of validation context. It contains only a reference to the object being validated.
            ICollection<ValidationResult> results = new List<ValidationResult>(); // Will contain the results of the validation
            bool isValid = Validator.TryValidateObject(daycare, vc, results, true); // Validates the object and its properties using the previously created context.
            if (!isValid) {
                return BadRequest(results);
            }

            dbContext.Daycares.Add(daycare);
            int result = dbContext.SaveChanges();
            if (result < 1)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            // Create default questions for the new daycare
            var question = new Question
            {
                name = "",
                daycareId = daycare.id,
            };

            Debug.WriteLine(question.id);

            question.name = "Domaine physique et moteur";
            dbContext.Questions.Add(question);
            int resultQuestion1= dbContext.SaveChanges();

            question.name = "Domaine social et affectif";
            question.id = 0;
            dbContext.Questions.Add(question);
            int resultQuestion2 = dbContext.SaveChanges();

            question.name = "Domaine langagier";
            question.id = 0;
            dbContext.Questions.Add(question);
            int resultQuestion3 = dbContext.SaveChanges();

            question.name = "Domaine cognitif";
            question.id = 0;
            dbContext.Questions.Add(question);
            int resultQuestion4 = dbContext.SaveChanges();


            return StatusCode(StatusCodes.Status201Created, daycare);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var daycare = dbContext.Daycares.Find(id);

            if(HttpContext != null)
                HttpContext.Session.SetString("daycareId", id.ToString());

            if (daycare == null)
            {
                return NotFound();
            }
            return Ok(daycare);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(int id, [FromBody] AddDaycareDto daycareDto)
        {
            var daycare = dbContext.Daycares.Find(id);
            if (daycare == null)
            {
                return NotFound();
            }

            daycare.name = daycareDto.name;
            //user.email = userDto.email;
            //user.password = userDto.password;

            // Try to validate the model
            ValidationContext vc = new ValidationContext(daycare); // The simplest form of validation context. It contains only a reference to the object being validated.
            ICollection<ValidationResult> results = new List<ValidationResult>(); // Will contain the results of the validation
            bool isValid = Validator.TryValidateObject(daycare, vc, results, true); // Validates the object and its properties using the previously created context.
            if (!isValid)
            {
                return BadRequest(results);
            }

            dbContext.SaveChanges();
            return Ok(daycare);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            var daycare = dbContext.Daycares.Find(id);
            if (daycare == null)
            {
                return NotFound();
            }
            dbContext.Daycares.Remove(daycare);
            dbContext.SaveChanges();
            return Ok();
        }
    }
}
