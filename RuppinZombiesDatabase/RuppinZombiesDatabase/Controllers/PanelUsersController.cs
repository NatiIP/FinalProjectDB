using Microsoft.AspNetCore.Mvc;
using RuppinZombiesDatabase.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RuppinZombiesDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PanelUsersController : ControllerBase
    {
        // GET: api/<PanelUsersController>
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                return Ok(Models.PanelUser.GetPanelUsers());
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // GET api/<PanelUsersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet]
        [Route("GetUserInfo/{id}")]
        public ActionResult GetUserInfo(string id)
        {
            try
            {
                PanelUser u = new();
                return Ok(u.GetUserInfoById(id));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // POST api/<PanelUsersController>
        [HttpPost]
        [Route("RegisterPanelUser")]
        public ActionResult RegisterPanelUser([FromBody] PanelUser userData)
        {
            try
            {
                return Ok(userData.RegisterUser());
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [Route("UserLogIn/{email}")]
        public ActionResult UserLogIn(string email, [FromBody] string password)
        {
            try
            {
                PanelUser u = new PanelUser();
                string result = u.LogIn(email, password);

                if (result == "Invalid email or password")
                {
                    return Ok("Invalid email or password"); // Return "Invalid email or password" message
                }
                else
                {
                    return Ok(result); // Return result if user exists
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message); // Return 500 status code with exception message
            }
        }



        // PUT api/<PanelUsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PanelUsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
