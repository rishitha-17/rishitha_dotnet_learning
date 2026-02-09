using System.ComponentModel.DataAnnotations.Schema;
[Table("users")]
    public class User
    {
        [Column("id")]
        public int Id { get; set; }     
        [Column("user_name")]
        public string Username { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("password_hash")]
        public string PasswordHash { get; set; }

        [Column("role")]
        public string Role { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
        
        
    }
