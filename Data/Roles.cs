using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRestoranApi.Data
{
    [Table("roles")]
    public class Role
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("name")]
        public string Name { get; set; }

       
        public List<User> Users { get; set; }
    }
}
