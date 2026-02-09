using policy_management.Entities;
using policy_management.Data;
using Microsoft.EntityFrameworkCore;

namespace policy_management.Repositories;

public class PolicyRepository : IPolicyRepository
{
    private readonly PolicyDbContext _dbContext;
    
    public PolicyRepository(PolicyDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<Policy>> GetAllPoliciesAsync()
    {
        return await _dbContext.Policies.ToListAsync();
    }

    public async Task<IEnumerable<Policy>> GetPoliciesByStatusAsync(bool isActive)
    {
        return await _dbContext.Policies
            .Where(p => p.IsActive == isActive)
            .ToListAsync();
    }

    public async Task<Policy> GetPolicyByIdAsync(int id)
    {
        return await _dbContext.Policies
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Policy>> SearchPoliciesByAmountAsync(int minAmount, int maxAmount)
    {
        return await _dbContext.Policies
            .Where(p => p.PremiumAmount >= minAmount && p.PremiumAmount <= maxAmount)
            .ToListAsync();
    }
     public async Task<Policy> CreatePolicyAsync(Policy policy)
    {
        // Ensure DateTime fields are set to UTC
        policy.CreatedAt = DateTime.UtcNow;
        policy.UpdatedAt = DateTime.UtcNow;
        
        _dbContext.Set<Policy>().Add(policy);
        await _dbContext.SaveChangesAsync();
        return policy;
    }
    public async Task<Policy> UpdatePolicyAsync(Policy policy)
    {
        // Find the existing policy in the database
        var existingPolicy = await _dbContext.Policies.FirstOrDefaultAsync(p => p.Id == policy.Id);
        if (existingPolicy == null)
        {
            throw new InvalidOperationException("Policy not found");
        }

        // Update only the fields that should be modified, preserve CreatedAt
        existingPolicy.Name = policy.Name;
        existingPolicy.Description = policy.Description;
        existingPolicy.PremiumAmount = policy.PremiumAmount;
        existingPolicy.IsActive = policy.IsActive;
        existingPolicy.UpdatedAt = DateTime.UtcNow; // Always use UTC
        
        await _dbContext.SaveChangesAsync();
        return existingPolicy;
    }
}
