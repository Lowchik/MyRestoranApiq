using System.ComponentModel.DataAnnotations;

namespace MyRestoranApi.Dto
{
    public class CreateOrderRequest
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int StatusId { get; set; }

        [Required]
        public int OrderTypeId { get; set; }

        public int? EmployeeId { get; set; }

        public int? CourierId { get; set; }

        public string? DeliveryAddress { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
