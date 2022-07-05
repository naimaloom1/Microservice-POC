using Microsoft.AspNetCore.Mvc;
using UserService.Database;
using UserService.Repository;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }
       
        [HttpGet]
        [Route("CheckHealth")]
        public IActionResult Check()
        {
            return Ok("Running and Up!");
        }
        [HttpGet]
        public IQueryable Get()
        {
            return _userRepo.GetUsers();
        }
       
        [HttpGet("{id}")]
        public User Get(int id)
        {
            return _userRepo.GetUser(id);
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            if (user == null)
                return BadRequest(ModelState);
            if (_userRepo.UserExists(user.MobileNo))
            {
                ModelState.AddModelError("", "Mobile No already Exist");
                return StatusCode(500, ModelState);
            }
            user.UpdatedOn = DateTime.Today;
            if (!_userRepo.CreateUser(user))
            {
                ModelState.AddModelError("", $"Something went wrong while saving movie record of {user.MobileNo}");
                return StatusCode(500, ModelState);
            }

            return Ok(user);

        }

        // PUT api/<UserController>/5
        [HttpPut("{Id:int}")]
        public IActionResult Update(int Id, [FromBody] User user)
        {
            if (user == null || Id != user.UserId)
                return BadRequest(ModelState);

            if (!_userRepo.UpdateUser(user))
            {
                ModelState.AddModelError("", $"Something went wrong while updating movie : {user.MobileNo}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{Id:int}")]
        public IActionResult Delete(int Id)
        {
            if (!_userRepo.UserExists(Id))
            {
                return NotFound();
            }

            var userobj = _userRepo.GetUser(Id);

            if (!_userRepo.DeleteUser(userobj))
            {
                ModelState.AddModelError("", $"Something went wrong while deleting movie : {userobj.MobileNo}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
