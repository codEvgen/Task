using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Test_task.Data;
using Test_task.Models;
using Test_task.Repositories;


namespace Test_task.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context,IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return Ok(await _userRepository.GetAllUsersAsync());
        }

       
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userRepository.GetUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> PostUser(UserDTO userDto)
        {
            if (!await _userRepository.IsLoginUniqueAsync(userDto.Login))
            {
                return Conflict(new { message = "User with this login already exists" });
            }

            var adminGroup = _context.UserGroups.First(group => group.Code == "Admin");
            if (userDto.UserGroupId == adminGroup.Id &&
                !await _userRepository.CheckAdminUserLimitAsync())
            {
                return BadRequest(new { message = "Cannot create more than one user with Admin role" });
            }

            await Task.Delay(TimeSpan.FromSeconds(5));

            if (!await _userRepository.IsLoginUniqueAsync(userDto.Login))
            {
                return Conflict(new { message = "User with this login already exists" });
            }

            var user = new User
            {
                Login = userDto.Login,
                Password = userDto.Password,
                UserGroupId = userDto.UserGroupId,
                UserStateId = _context.UserStates.First(state => state.Code == "Active").Id,
                CreatedDate = DateTime.UtcNow
            };

            var createdUser = await _userRepository.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
        }
        
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.BlockUserAsync(id);
            if (user== null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
