using policy_management.DTOs;

namespace policy_management.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> RegisterAsync(RegisterDTO registerDto);
        Task<LoginResponseDTO> LoginAsync(LoginDTO loginDto);
    }
}

