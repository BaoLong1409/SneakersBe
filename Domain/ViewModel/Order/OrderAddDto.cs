using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel.Order
{
    public class OrderAddDto
    {
        public required DateTime OrderDate { get; set; }
        public DateTime? ShippingDate { get; set; }
        public required Decimal TotalMoney { get; set; }
        public String? Note { get; set; }
        public String? FullName { get; set; }
        [RegularExpression(@"^(03|05|07|08|09)\d{8}$")]
        public String? PhoneNumber { get; set; }
        public String? ShippingAddress { get; set; }
        public Guid? UserId { get; set; }
        public required Guid ShippingId { get; set; }
        public Guid? ShippingInforId { get; set; }
        public required Guid PaymentId { get; set; }
    }
}
