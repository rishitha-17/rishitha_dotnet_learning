using policy_management.Entities;   
using policy_management.Services;
using policy_management.Repositories;
using policy_management.DTOs;
using Microsoft.AspNetCore.Identity;
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
            var hasher = new PasswordHasher<UserDTO>();
            user.password_hash = hasher.HashPassword(user, user.password_hash);
            return await _usersRepository.CreateUserAsync(user);
        }
        public async Task<User> UpdateUserAsync(User user)
        {
            return await _usersRepository.UpdateUserAsync(user);
        }
    }
}