using policy_management.DTOs;
using policy_management.Entities;
using policy_management.Repositories;

namespace policy_management.Services
{
    public class PolicyEnrollmentService : IPolicyEnrollmentService
    {
        private readonly IPolicyEnrollmentRepository _enrollmentRepository;
        private readonly IPolicyRepository _policyRepository;
        private readonly IUsersRepository _usersRepository;

        public PolicyEnrollmentService(
            IPolicyEnrollmentRepository enrollmentRepository,
            IPolicyRepository policyRepository,
            IUsersRepository usersRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _policyRepository = policyRepository;
            _usersRepository = usersRepository;
        }

        public async Task<PolicyEnrollmentDTO> RequestEnrollmentAsync(int userId, EnrollmentRequestDTO request)
        {
            // Check if user exists
            var user = await _usersRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            // Check if policy exists and is active
            var policy = await _policyRepository.GetPolicyByIdAsync(request.PolicyId);
            if (policy == null)
            {
                throw new KeyNotFoundException("Policy not found");
            }

            if (!policy.IsActive)
            {
                throw new InvalidOperationException("Policy is not active");
            }

            // Check if user already enrolled in this policy
            var existingEnrollment = await _enrollmentRepository.GetExistingEnrollmentAsync(userId, request.PolicyId);
            if (existingEnrollment != null)
            {
                throw new InvalidOperationException("User already enrolled in this policy");
            }

            // Create new enrollment
            var enrollment = new PolicyEnrollment
            {
                UserId = userId,
                PolicyId = request.PolicyId,
                Status = "Pending",
                RequestedAt = DateTime.UtcNow
            };

            var createdEnrollment = await _enrollmentRepository.CreateEnrollmentAsync(enrollment);
            
            // Load related data for DTO
            var enrollmentWithDetails = await _enrollmentRepository.GetEnrollmentByIdAsync(createdEnrollment.Id);
            
            return MapToDTO(enrollmentWithDetails!);
        }

        public async Task<IEnumerable<PolicyEnrollmentDTO>> GetUserEnrollmentsAsync(int userId)
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsByUserAsync(userId);
            return enrollments.Select(MapToDTO);
        }

        public async Task<IEnumerable<PolicyEnrollmentDTO>> GetPendingEnrollmentsAsync()
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsByStatusAsync("Pending");
            return enrollments.Select(MapToDTO);
        }

        public async Task<IEnumerable<PolicyEnrollmentDTO>> GetAllEnrollmentsAsync()
        {
            var enrollments = await _enrollmentRepository.GetAllEnrollmentsAsync();
            return enrollments.Select(MapToDTO);
        }

        public async Task<PolicyEnrollmentDTO> ApproveEnrollmentAsync(int enrollmentId, string? adminComments)
        {
            var enrollment = await _enrollmentRepository.GetEnrollmentByIdAsync(enrollmentId);
            if (enrollment == null)
            {
                throw new KeyNotFoundException("Enrollment not found");
            }

            if (enrollment.Status != "Pending")
            {
                throw new InvalidOperationException("Enrollment is not in pending status");
            }

            enrollment.Status = "Approved";
            enrollment.ApprovedAt = DateTime.UtcNow;
            enrollment.AdminComments = adminComments;

            var updatedEnrollment = await _enrollmentRepository.UpdateEnrollmentAsync(enrollment);
            return MapToDTO(updatedEnrollment);
        }

        public async Task<PolicyEnrollmentDTO> RejectEnrollmentAsync(int enrollmentId, string? adminComments)
        {
            var enrollment = await _enrollmentRepository.GetEnrollmentByIdAsync(enrollmentId);
            if (enrollment == null)
            {
                throw new KeyNotFoundException("Enrollment not found");
            }

            if (enrollment.Status != "Pending")
            {
                throw new InvalidOperationException("Enrollment is not in pending status");
            }

            enrollment.Status = "Rejected";
            enrollment.RejectedAt = DateTime.UtcNow;
            enrollment.AdminComments = adminComments;

            var updatedEnrollment = await _enrollmentRepository.UpdateEnrollmentAsync(enrollment);
            return MapToDTO(updatedEnrollment);
        }

        private static PolicyEnrollmentDTO MapToDTO(PolicyEnrollment enrollment)
        {
            return new PolicyEnrollmentDTO
            {
                Id = enrollment.Id,
                UserId = enrollment.UserId,
                PolicyId = enrollment.PolicyId,
                Status = enrollment.Status,
                RequestedAt = enrollment.RequestedAt,
                ApprovedAt = enrollment.ApprovedAt,
                RejectedAt = enrollment.RejectedAt,
                AdminComments = enrollment.AdminComments,
                UserName = enrollment.User?.Username,
                UserEmail = enrollment.User?.Email,
                PolicyName = enrollment.Policy?.Name,
                PremiumAmount = enrollment.Policy?.PremiumAmount
            };
        }
    }
}

