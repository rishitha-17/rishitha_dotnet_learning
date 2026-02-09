using policy_management.DTOs;
using policy_management.Entities;

namespace policy_management.Services
{
    public interface IPolicyEnrollmentService
    {
        Task<PolicyEnrollmentDTO> RequestEnrollmentAsync(int userId, EnrollmentRequestDTO request);
        Task<IEnumerable<PolicyEnrollmentDTO>> GetUserEnrollmentsAsync(int userId);
        Task<IEnumerable<PolicyEnrollmentDTO>> GetPendingEnrollmentsAsync();
        Task<IEnumerable<PolicyEnrollmentDTO>> GetAllEnrollmentsAsync();
        Task<PolicyEnrollmentDTO> ApproveEnrollmentAsync(int enrollmentId, string? adminComments);
        Task<PolicyEnrollmentDTO> RejectEnrollmentAsync(int enrollmentId, string? adminComments);
    }
}