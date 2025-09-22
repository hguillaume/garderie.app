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
    public class AnswersController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public AnswersController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbContext.Database.EnsureCreated();
        }

        [HttpGet(Name = "GetAnswers")]
        //[Route("daycare/{daycareId}/kid/{kidId}")]
        public IActionResult Get([FromQuery] int daycareId, [FromQuery] int kidId)
        {
            //string daycareId;
            //if (HttpContext == null || HttpContext.Session == null)
            //{
            //    daycareId = "0"; // Default value if session is not available
            //}
            //else
            //{
            //    daycareId = HttpContext.Session.GetString("daycareId");
            //    if (string.IsNullOrEmpty(daycareId))
            //    {
            //        daycareId = "0";
            //    }
            //}

            //var allDaycares = dbContext.Daycares.ToList();
            //var allDaycares = dbContext.Users
            //    .SelectMany(user => user.Daycares)
            //    //.Where(a => a.userId == 1)
            //    .Where(a => a.userId == int.Parse(daycareId))
            //    .ToList();
            var allAnswers = dbContext.Kids
                .Where(kid => kid.daycareId == daycareId)
                .Where(kid => kid.id == kidId)
                .SelectMany(answer => answer.Answers)
                //.Where(a => a.userId == 1)
                //.Where(answer => answer.id == int.Parse(daycareId))
                .ToList();
            return Ok(allAnswers);
        }

        [HttpPost]
        public IActionResult Add([FromBody] AddAnswerDto answerDto)
        {
            var answer = new Answer
            {
                description = answerDto.description,
                kidId = answerDto.kidId,
                questionId = answerDto.questionId,
                daycareId = answerDto.daycareId,
                //date = DateTime.Now
            };

            // Try to validate the model
            ValidationContext vc = new ValidationContext(answer); // The simplest form of validation context. It contains only a reference to the object being validated.
            ICollection<ValidationResult> results = new List<ValidationResult>(); // Will contain the results of the validation
            bool isValid = Validator.TryValidateObject(answer, vc, results, true); // Validates the object and its properties using the previously created context.
            if (!isValid) {
                return BadRequest(results);
            }

            dbContext.Answers.Add(answer);
            int result = dbContext.SaveChanges();
            if (result < 1)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return StatusCode(StatusCodes.Status201Created, answer);
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
        public IActionResult Update(int id, [FromBody] AddAnswerDto answerDto)
        {
            var answer = dbContext.Answers.Find(id);
            if (answer == null)
            {
                return NotFound();
            }

            answer.name = answerDto.name;

            // Try to validate the model
            ValidationContext vc = new ValidationContext(answer); // The simplest form of validation context. It contains only a reference to the object being validated.
            ICollection<ValidationResult> results = new List<ValidationResult>(); // Will contain the results of the validation
            bool isValid = Validator.TryValidateObject(answer, vc, results, true); // Validates the object and its properties using the previously created context.
            if (!isValid)
            {
                return BadRequest(results);
            }

            dbContext.SaveChanges();
            return Ok(answer);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            var answer = dbContext.Answers.Find(id);
            if (answer == null)
            {
                return NotFound();
            }
            dbContext.Answers.Remove(answer);
            dbContext.SaveChanges();
            return Ok();
        }
    }
}
