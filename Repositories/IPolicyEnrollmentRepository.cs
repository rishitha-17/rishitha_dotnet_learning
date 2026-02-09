using policy_management.Entities;

namespace policy_management.Repositories
{
    public interface IPolicyEnrollmentRepository
    {
        Task<PolicyEnrollment> CreateEnrollmentAsync(PolicyEnrollment enrollment);
        Task<PolicyEnrollment?> GetEnrollmentByIdAsync(int id);
        Task<IEnumerable<PolicyEnrollment>> GetEnrollmentsByUserAsync(int userId);
        Task<IEnumerable<PolicyEnrollment>> GetEnrollmentsByStatusAsync(string status);
        Task<IEnumerable<PolicyEnrollment>> GetAllEnrollmentsAsync();
        Task<PolicyEnrollment?> GetExistingEnrollmentAsync(int userId, int policyId);
        Task<PolicyEnrollment> UpdateEnrollmentAsync(PolicyEnrollment enrollment);
    }
}