using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace policy_management.Entities
{
    [Table("policy_enrollments")]
    public class PolicyEnrollment
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        [Required]
        public int UserId { get; set; }

        [Column("policy_id")]
        [Required]
        public int PolicyId { get; set; }

        [Column("status")]
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

        [Column("requested_at")]
        [Required]
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

        [Column("approved_at")]
        public DateTime? ApprovedAt { get; set; }

        [Column("rejected_at")]
        public DateTime? RejectedAt { get; set; }

        [Column("admin_comments")]
        [StringLength(500)]
        public string? AdminComments { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("PolicyId")]
        public virtual Policy Policy { get; set; }
    }
}

