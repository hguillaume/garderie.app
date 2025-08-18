using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using garderie.app2.Server.Data;
using garderie.app2.Server.Models;
using garderie.app2.Server.Models.Entities;

namespace garderie.app2.Server.Controllers
{
    // localhost:port/api/Kids
    [Route("api/[controller]")]
    [ApiController]
    public class KidsController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public KidsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbContext.Database.EnsureCreated();
        }

        [HttpGet(Name = "GetKids")]
        public IActionResult Get()
        {
            string daycareId;
            if (HttpContext == null || HttpContext.Session == null)
            {
                daycareId = "0"; // Default value if session is not available
            }
            else
            {
                daycareId = HttpContext.Session.GetString("daycareId");
                if (string.IsNullOrEmpty(daycareId))
                {
                    daycareId = "0";
                }
            }

            //var allDaycares = dbContext.Daycares.ToList();
            //var allDaycares = dbContext.Users
            //    .SelectMany(user => user.Daycares)
            //    //.Where(a => a.userId == 1)
            //    .Where(a => a.userId == int.Parse(daycareId))
            //    .ToList();
            var allKids = dbContext.Daycares
                .SelectMany(daycare => daycare.Kids)
                //.Where(a => a.userId == 1)
                .Where(a => a.daycareId == int.Parse(daycareId))
                .ToList();
            return Ok(allKids);
        }

        [HttpPost]
        public IActionResult Add([FromBody] AddKidDto kidDto)
        {
            var kid = new Kid
            {
                name = kidDto.name,
                daycareId = kidDto.daycareId,
            };

            // Try to validate the model
            ValidationContext vc = new ValidationContext(kid); // The simplest form of validation context. It contains only a reference to the object being validated.
            ICollection<ValidationResult> results = new List<ValidationResult>(); // Will contain the results of the validation
            bool isValid = Validator.TryValidateObject(kid, vc, results, true); // Validates the object and its properties using the previously created context.
            if (!isValid) {
                return BadRequest(results);
            }

            dbContext.Kids.Add(kid);
            int result = dbContext.SaveChanges();
            if (result < 1)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return StatusCode(StatusCodes.Status201Created, kid);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var kid = dbContext.Kids.Find(id);
            if (kid == null)
            {
                return NotFound();
            }
            return Ok(kid);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(int id, [FromBody] AddKidDto kidDto)
        {
            var kid = dbContext.Kids.Find(id);
            if (kid == null)
            {
                return NotFound();
            }

            kid.name = kidDto.name;

            // Try to validate the model
            ValidationContext vc = new ValidationContext(kid); // The simplest form of validation context. It contains only a reference to the object being validated.
            ICollection<ValidationResult> results = new List<ValidationResult>(); // Will contain the results of the validation
            bool isValid = Validator.TryValidateObject(kid, vc, results, true); // Validates the object and its properties using the previously created context.
            if (!isValid)
            {
                return BadRequest(results);
            }

            dbContext.SaveChanges();
            return Ok(kid);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            var kid = dbContext.Kids.Find(id);
            if (kid == null)
            {
                return NotFound();
            }
            dbContext.Kids.Remove(kid);
            dbContext.SaveChanges();
            return Ok();
        }
    }
}
