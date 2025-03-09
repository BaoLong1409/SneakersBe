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
        public required DateTime OrderDate { get; set; }
        public DateTime? ShippingDate { get; set; }
        [Required]
        public required Decimal TotalMoney { get; set; }
        public String? Note { get; set; }
        public String? FullName { get; set; }
        [RegularExpression(@"^(03|05|07|08|09)\d{8}$")]
        public String? PhoneNumber { get; set; }
        public String? ShippingAddress { get; set; }
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        [Required]
        public required Guid ShippingId { get; set; }
        public Shipping? Shipping { get; set; }
        public Guid? ShippingInforId { get; set; }
        public ShippingInfor? ShippingInfor { get; set; }
        [Required]
        public required Guid PaymentId { get; set; }
        public Payment? Payment { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
    }
}
