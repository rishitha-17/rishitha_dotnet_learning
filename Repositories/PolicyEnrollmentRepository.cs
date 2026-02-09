using Microsoft.EntityFrameworkCore;
using policy_management.Data;
using policy_management.Entities;

namespace policy_management.Repositories
{
    public class PolicyEnrollmentRepository : IPolicyEnrollmentRepository
    {
        private readonly PolicyDbContext _context;

        public PolicyEnrollmentRepository(PolicyDbContext context)
        {
            _context = context;
        }

        public async Task<PolicyEnrollment> CreateEnrollmentAsync(PolicyEnrollment enrollment)
        {
            // Ensure DateTime fields are set to UTC
            enrollment.RequestedAt = DateTime.UtcNow;
            
            _context.PolicyEnrollments.Add(enrollment);
            await _context.SaveChangesAsync();
            return enrollment;
        }

        public async Task<PolicyEnrollment?> GetEnrollmentByIdAsync(int id)
        {
            return await _context.PolicyEnrollments
                .Include(pe => pe.User)
                .Include(pe => pe.Policy)
                .FirstOrDefaultAsync(pe => pe.Id == id);
        }

        public async Task<IEnumerable<PolicyEnrollment>> GetEnrollmentsByUserAsync(int userId)
        {
            return await _context.PolicyEnrollments
                .Include(pe => pe.User)
                .Include(pe => pe.Policy)
                .Where(pe => pe.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<PolicyEnrollment>> GetEnrollmentsByStatusAsync(string status)
        {
            return await _context.PolicyEnrollments
                .Include(pe => pe.User)
                .Include(pe => pe.Policy)
                .Where(pe => pe.Status == status)
                .OrderBy(pe => pe.RequestedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PolicyEnrollment>> GetAllEnrollmentsAsync()
        {
            return await _context.PolicyEnrollments
                .Include(pe => pe.User)
                .Include(pe => pe.Policy)
                .OrderByDescending(pe => pe.RequestedAt)
                .ToListAsync();
        }

        public async Task<PolicyEnrollment?> GetExistingEnrollmentAsync(int userId, int policyId)
        {
            return await _context.PolicyEnrollments
                .FirstOrDefaultAsync(pe => pe.UserId == userId && pe.PolicyId == policyId);
        }

        public async Task<PolicyEnrollment> UpdateEnrollmentAsync(PolicyEnrollment enrollment)
        {
            var existingEnrollment = await _context.PolicyEnrollments.FirstOrDefaultAsync(e => e.Id == enrollment.Id);
            if (existingEnrollment == null)
            {
                throw new InvalidOperationException("Policy enrollment not found");
            }

            // Update only the fields that should be modified, always ensure DateTime values are UTC
            existingEnrollment.Status = enrollment.Status;
            existingEnrollment.AdminComments = enrollment.AdminComments;
            
            // Set approval/rejection timestamps to UTC if status changed
            if (enrollment.Status == "Approved" && existingEnrollment.ApprovedAt == null)
            {
                existingEnrollment.ApprovedAt = DateTime.UtcNow;
                existingEnrollment.RejectedAt = null; // Clear rejected timestamp
            }
            else if (enrollment.Status == "Rejected" && existingEnrollment.RejectedAt == null)
            {
                existingEnrollment.RejectedAt = DateTime.UtcNow;
                existingEnrollment.ApprovedAt = null; // Clear approved timestamp
            }

            await _context.SaveChangesAsync();
            return existingEnrollment;
        }
    }
}

