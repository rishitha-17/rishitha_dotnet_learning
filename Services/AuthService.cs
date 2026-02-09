using Microsoft.EntityFrameworkCore;
using policy_management.DTOs;
using policy_management.Entities;
using policy_management.Repositories;
using policy_management.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace policy_management.Services
{
    public class AuthService : IAuthService
    {
        private readonly PolicyDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthService(PolicyDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<LoginResponseDTO> RegisterAsync(RegisterDTO registerDto)
        {
            // Check if email already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email);
            if (existingUser != null)
            {
                throw new ArgumentException("Email already exists");
            }

            // Create user
            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                Role = registerDto.Role,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Hash password
            var hasher = new PasswordHasher<RegisterDTO>();
            user.PasswordHash = hasher.HashPassword(registerDto, registerDto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Generate token
            var token = _jwtService.GenerateToken(user.Email, user.Role, user.Id);

            return new LoginResponseDTO
            {
                Token = token,
                Email = user.Email,
                Username = user.Username,
                Role = user.Role,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginDTO loginDto)
        {
            // Find user
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            // Check password
            var hasher = new PasswordHasher<User>();
            if (hasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password) == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            // Generate token
            var token = _jwtService.GenerateToken(user.Email, user.Role, user.Id);

            return new LoginResponseDTO
            {
                Token = token,
                Email = user.Email,
                Username = user.Username,
                Role = user.Role,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };
        }
    }
}

