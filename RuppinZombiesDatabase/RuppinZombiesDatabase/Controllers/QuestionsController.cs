using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RuppinZombiesDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        // GET: api/<QuestionsController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(Models.Question.GetAllQuestions());
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error " + ex.Message });
            }
        }

        // GET api/<QuestionsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<QuestionsController>
        [HttpPost]
        public IActionResult Post([FromBody] Models.Question q)
        {
            try
            {
                return q.Insert() ? Ok(new {message="Inserted"}) : BadRequest(new { message = "Server Error" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error " + ex.Message });
            }
        }

        // PUT api/<QuestionsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<QuestionsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
