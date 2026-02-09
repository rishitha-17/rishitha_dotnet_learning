using policy_management.Entities;
using policy_management.Repositories;

namespace policy_management.Services;

public class PolicyService : IPolicyService
{
    private readonly IPolicyRepository policyRepository;
    public PolicyService(IPolicyRepository _policyRepository)
    {
        this.policyRepository = _policyRepository;
    }

    public async Task<IEnumerable<Policy>> GetAllPoliciesAsync()
    {
        return await policyRepository.GetAllPoliciesAsync();
    }

    public async Task<IEnumerable<Policy>> GetPoliciesByStatusAsync(bool isActive)
    {
       return await policyRepository.GetPoliciesByStatusAsync(isActive);
    }

    public async Task<Policy> GetPolicyByIdAsync(int id)
    {
        return await policyRepository.GetPolicyByIdAsync(id);
    }
    public async Task<IEnumerable<Policy>> SearchPoliciesByAmountAsync(int minAmount, int maxAmount)
    {
       return await policyRepository.SearchPoliciesByAmountAsync(minAmount, maxAmount);
    }
    public async Task<Policy> CreatePolicyAsync(Policy policy)
    {
    
        return await policyRepository.CreatePolicyAsync(policy);
    }
    public async Task<Policy> UpdatePolicyAsync(Policy policy)
    {
        return await policyRepository.UpdatePolicyAsync(policy);
    }
   
}