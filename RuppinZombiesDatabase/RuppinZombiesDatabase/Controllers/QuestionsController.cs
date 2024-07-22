using Microsoft.AspNetCore.Mvc;
using RuppinZombiesDatabase.Models;

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

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            try
            {
                return Ok(Models.Question.GetUserQuestions(id));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error " + ex.Message });
            }
        }

        [HttpGet("insights/{questionId}")]
        public IActionResult GetQuestionInsights(int questionId)
        {
            try
            {
                return Ok(QuestionInsights.GetUserQuestionInsight(questionId));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error " + ex.Message });
            }
        }
        
        [HttpGet("widgetInsights/{lecturerId}")]
        public IActionResult GetAnswersInsights(int lecturerId)
        {
            try
            {
                return Ok(QuestionsAnsInsights.GetUserQuestionsAnsInsights(lecturerId));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error " + ex.Message });
            }
        }

        // POST api/<QuestionsController>
        [HttpPost]
        public IActionResult Post([FromBody] Question q)
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
        public IActionResult Delete(int id)
        {
            try
            {
                return Question.DeleteQuestion(id) ? Ok(new { message = "deleted" }) : BadRequest(new { message = "Server Error" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error " + ex.Message });
            }
        }
        
        // DELETE api/<QuestionsController>/5
        [HttpDelete]
        [Route("DeleteSelected/{ids}")]
        public IActionResult DeleteSelected(string ids)
        {
            try
            {
                return Question.DeleteQuestions(ids) ? Ok(new { message = "deleted" }) : BadRequest(new { message = "Server Error" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error " + ex.Message });
            }
        }
    }
}
