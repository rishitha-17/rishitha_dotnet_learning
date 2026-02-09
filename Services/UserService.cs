using policy_management.Entities;   
using policy_management.Services;
using policy_management.Repositories;
using policy_management.DTOs;
using Microsoft.AspNetCore.Identity;
using policy_management.Utilities;
namespace policy_management.Services
{
    public class UserService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;

        public UserService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _usersRepository.GetAllUsersAsync();
        }
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _usersRepository.GetUserByIdAsync(id);
        }
        public async Task<User> CreateUserAsync(UserDTO user)
        {
            var hashedPassword = PasswordHasher.HashPassword(user.password_hash);
            user.password_hash = hashedPassword;
            return await _usersRepository.CreateUserAsync(user);
        }
        public async Task<User> UpdateUserAsync(User user)
        {
            return await _usersRepository.UpdateUserAsync(user);
        }
    }
}