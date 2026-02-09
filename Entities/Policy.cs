using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace policy_management.Entities;

[Table("policy")]
    public class Policy
    {
        [Column("id")]
        public int Id { get; set; }     
        [Column("policy_name")]
        [Required(ErrorMessage = "Policy name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Policy name must be between 2 and 100 characters")]
        public string Name { get; set; }
        [Column("premium_amount")]
        [Range(1, 100000, ErrorMessage = "Premium amount must be greater than 0")]
        public int PremiumAmount { get; set; }
        [Column("description")]
        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 500 characters")]
        public string Description { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
