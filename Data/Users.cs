using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace MyRestoranApi.Data
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("email")]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [Column("password_hash")]
        public string PasswordHash { get; set; }

        [Required]
        [Column("role_id")]
        public int RoleId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("RoleId")]
        public Role? Role { get; set; }

    }
}
