using System.ComponentModel.DataAnnotations.Schema;

namespace policy_management.DTOs
{
    public class PolicyDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public int PremiumAmount { get; set; }

        public bool IsActive { get; set; }
    }

    public class PolicyStatusDTO
    {
        public bool IsActive { get; set; }
    }
}