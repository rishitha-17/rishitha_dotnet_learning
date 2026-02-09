using policy_management.DTOs;
using policy_management.Entities;
namespace policy_management.Services
{
    public interface IUsersService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(UserDTO user);
        Task<User> UpdateUserAsync(User user);
    }
}