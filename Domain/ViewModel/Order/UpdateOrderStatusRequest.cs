using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.Order
{
    public class UpdateOrderStatusRequest
    {
        public Guid OrderId { get; set; }
        public required string OrderStatus { get; set; }
    }
}
