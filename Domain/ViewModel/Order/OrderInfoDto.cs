using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.Order
{
    public class OrderInfoDto
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public required Decimal TotalMoney { get; set; }
        public string? Note { get; set; }
        public required string FullName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string ShippingAddress { get; set; }
        public required string ShippingName { get; set; }
        public Decimal ShippingPrice { get; set; }
        public required int MinimumDeliveredTime { get; set; }
        public required int MaximumDeliveredTime { get; set; }
        public required string PaymentName { get; set; }
        public required List<OrderStatusHistoryDto> StatusHistories { get; set; }

    }
}
