using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using garderie.app2.Server.Data;
using garderie.app2.Server.Models;
using garderie.app2.Server.Models.Entities;

namespace garderie.app2.Server.Controllers
{
    // localhost:port/api/Users
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

        [HttpGet(Name = "GetDaycares")]
        public IActionResult Get()
        {
            var allDaycares = dbContext.Daycares.ToList();
            return Ok(allDaycares);
        }

        [HttpPost]
        public IActionResult Add([FromBody] AddDaycareDto daycareDto)
        {
            var daycare = new Daycare
            {
                name = daycareDto.name,
                user_id = daycareDto.user_id,
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
            return StatusCode(StatusCodes.Status201Created, daycare);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var daycare = dbContext.Daycares.Find(id);
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
