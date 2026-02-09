using Microsoft.AspNetCore.Mvc;
using policy_management.Repositories;
using policy_management.Services;
using policy_management.Entities;
using policy_management.DTOs;

namespace policy_management
{
    [ApiController]
    [Route("api/users")]

    public class UserController : ControllerBase
    {
        private readonly IUsersService userService;
        public UserController(IUsersService _userService)
        {
            this.userService = _userService;
        }
        [HttpGet (Name = "GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await userService.GetAllUsersAsync();
            return Ok(users);
        }
        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await userService.GetUserByIdAsync(id);
            return Ok(user);
        }
        [HttpPost(Name = "CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO user)
        {
            var createdUser = await userService.CreateUserAsync(user);
            return CreatedAtRoute("GetUserById", new { id = createdUser.Id }, createdUser);
        }
        [HttpPut("{id}", Name = "UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] UserDTO userDTO)
        {
            var existingUser = await userService.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }
            existingUser.Username = userDTO.User_Name;
            existingUser.Email = userDTO.Email;
            existingUser.Role = userDTO.Role;
            existingUser.PasswordHash = userDTO.password_hash;
            var updatedUser = await userService.UpdateUserAsync(existingUser);
            return Ok(updatedUser);
        }
    }

}