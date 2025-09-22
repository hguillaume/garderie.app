using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using garderie.app2.Server.Data;
using garderie.app2.Server.Models;
using garderie.app2.Server.Models.Entities;

namespace garderie.app2.Server.Controllers
{
    // localhost:port/api/Questions
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public QuestionsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbContext.Database.EnsureCreated();
        }

        [HttpGet(Name = "GetQuestions")]
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
            var allQuestions = dbContext.Daycares
                .SelectMany(daycare => daycare.Questions)
                //.Where(a => a.userId == 1)
                .Where(a => a.daycareId == int.Parse(daycareId))
                .ToList();
            return Ok(allQuestions);
        }

        [HttpPost]
        public IActionResult Add([FromBody] AddQuestionDto questionDto)
        {
            var question = new Question
            {
                name = questionDto.name,
                daycareId = questionDto.daycareId,
            };

            // Try to validate the model
            ValidationContext vc = new ValidationContext(question); // The simplest form of validation context. It contains only a reference to the object being validated.
            ICollection<ValidationResult> results = new List<ValidationResult>(); // Will contain the results of the validation
            bool isValid = Validator.TryValidateObject(question, vc, results, true); // Validates the object and its properties using the previously created context.
            if (!isValid) {
                return BadRequest(results);
            }

            dbContext.Questions.Add(question);
            int result = dbContext.SaveChanges();
            if (result < 1)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return StatusCode(StatusCodes.Status201Created, question);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var question = dbContext.Questions.Find(id);
            if (question == null)
            {
                return NotFound();
            }
            return Ok(question);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(int id, [FromBody] AddQuestionDto questionDto)
        {
            var question = dbContext.Questions.Find(id);
            if (question == null)
            {
                return NotFound();
            }

            question.name = questionDto.name;

            // Try to validate the model
            ValidationContext vc = new ValidationContext(question); // The simplest form of validation context. It contains only a reference to the object being validated.
            ICollection<ValidationResult> results = new List<ValidationResult>(); // Will contain the results of the validation
            bool isValid = Validator.TryValidateObject(question, vc, results, true); // Validates the object and its properties using the previously created context.
            if (!isValid)
            {
                return BadRequest(results);
            }

            dbContext.SaveChanges();
            return Ok(question);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            var question = dbContext.Questions.Find(id);
            if (question == null)
            {
                return NotFound();
            }
            dbContext.Questions.Remove(question);
            dbContext.SaveChanges();
            return Ok();
        }
    }
}
