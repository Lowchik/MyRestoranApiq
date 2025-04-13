using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRestoranApi.Data
{
    [Table("couriers")]
    public class Courier
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        [Required]
        public int UserId { get; set; }

        [Column("first_name")]
        [Required]
        [MaxLength(100)]
        public required string FirstName { get; set; }

        [Column("last_name")]
        [Required]
        [MaxLength(100)]
        public required string LastName { get; set; }

        [Column("phone")]
        [MaxLength(20)]
        public string? Phone { get; set; }

        [Column("vehicle_type")]
        [MaxLength(50)]
        public string? VehicleType { get; set; }

        
        
    }
}
