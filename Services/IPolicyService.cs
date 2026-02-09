using policy_management.Entities;
namespace policy_management.Services
{
    public interface IPolicyService
    {
    // Define methods for policy management
        Task<IEnumerable<Policy>> GetAllPoliciesAsync();
        Task<Policy> GetPolicyByIdAsync(int id);
        Task<IEnumerable<Policy>> SearchPoliciesByAmountAsync(int minAmount, int maxAmount);
        Task<IEnumerable<Policy>> GetPoliciesByStatusAsync(bool isActive);
        Task<Policy> CreatePolicyAsync(Policy policy);
        Task<Policy> UpdatePolicyAsync(Policy policy);
    }
}

