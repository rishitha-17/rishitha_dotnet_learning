
using Microsoft.EntityFrameworkCore;
using policy_management.Data;
using policy_management.DTOs;
using policy_management.Entities;

namespace policy_management.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly PolicyDbContext _context;

        public UsersRepository(PolicyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<User> CreateUserAsync(UserDTO user)
        {
            if(user.Role.ToLower() != "admin" && user.Role.ToLower() != "user")
            {
                throw new ArgumentException("Invalid role specified. Role must be either 'Admin' or 'User'.");
            }
            var newUser = new User
            {
                Username = user.User_Name,
                Email = user.Email,
                PasswordHash = user.password_hash,
                Role = user.Role,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser; 
        }
        public async Task<User> UpdateUserAsync(User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (existingUser == null)
            {
                throw new InvalidOperationException("User not found");
            }

            // Update only the fields that should be modified, preserve CreatedAt
            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.PasswordHash = user.PasswordHash;
            existingUser.Role = user.Role;
            existingUser.UpdatedAt = DateTime.UtcNow; // Always use UTC

            await _context.SaveChangesAsync();
            return existingUser;
        }
    }
    
}