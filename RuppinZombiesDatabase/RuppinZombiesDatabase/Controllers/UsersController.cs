using Microsoft.AspNetCore.Mvc;
using RuppinZombiesDatabase.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RuppinZombiesDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsersController>
        [HttpPost]
        public IActionResult Post([FromBody] Models.User UserToInsert)
        {
            try
            {
                return Ok(UserToInsert.Insert());
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error " + ex.Message });
            }
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
        [HttpPost("InsertUserAnswer/UserID/{UserID}/QuestionID/{QuestionID}/UserAnswer/{UserAnswer}")]
        public IActionResult InsertUserAnswer(string UserID, int QuestionID, int UserAnswer)
        {
            try
            {
                return Models.User.InsertUserAnswer(UserID, QuestionID, UserAnswer) ?
                    Ok(new { message = "Inserted" }) : BadRequest(new { message = "Server Error" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error " + ex.Message });
            }
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
