using System.ComponentModel.DataAnnotations;

namespace policy_management.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string User_Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string password_hash { get; set; }
    }

    public class RegisterDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }

    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }
    }

    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}