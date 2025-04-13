using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace MyRestoranApi.Data
{
    [Table("orders")]
    public class Order
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("customer_id")]
        [Required]
        public int CustomerId { get; set; }

        [Column("order_time")]
        public DateTime OrderTime { get; set; } = DateTime.UtcNow;

        [Column("status_id")]
        [Required]
        public int StatusId { get; set; }

        [Column("order_type_id")]
        [Required]
        public int OrderTypeId { get; set; }

        [Column("employee_id")]
        public int? EmployeeId { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("total_price")]
        [DataType(DataType.Currency)]
        public decimal? TotalPrice { get; set; }

        [Column("courier_id")]
        public int? CourierId { get; set; }

        [Column("delivery_address")]
        [MaxLength(500)]
        public string? DeliveryAddress { get; set; }

       
        public Customer? Customer { get; set; }
        public Courier? Courier { get; set; }
        public Employee? Employee { get; set; }
        public OrderType? OrderType { get; set; }
        public Status? Status { get; set; }
    }
}
