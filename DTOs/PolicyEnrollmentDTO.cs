namespace policy_management.DTOs
{
    public class PolicyEnrollmentDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PolicyId { get; set; }
        public string Status { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public DateTime? RejectedAt { get; set; }
        public string? AdminComments { get; set; }
        
        // Additional info for API responses
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? PolicyName { get; set; }
        public int? PremiumAmount { get; set; }
    }

    public class EnrollmentRequestDTO
    {
        public int PolicyId { get; set; }
    }

    public class AdminActionDTO
    {
        public string Action { get; set; } // "approve" or "reject"
        public string? Comments { get; set; }
    }
}

