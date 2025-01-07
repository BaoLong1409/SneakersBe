using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public required string Status { get; set; }
        [Required]
        public required DateTime OrderDate { get; set; }
        [Required]
        public required DateTime ShippingDate { get; set; }
        [Required]
        public required Decimal TotalMoney { get; set; }
        [Required]
        public required Guid UserId { get; set; }
        public User? User { get; set; }
        [Required]
        public required Guid ShippingId { get; set; }
        public Shipping? Shipping { get; set; }
        [Required]
        public required Guid PaymentId { get; set; }
        public Payment? Payment { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
