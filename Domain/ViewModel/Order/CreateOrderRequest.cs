using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.Order
{
    public class CreateOrderRequest
    {
        public required OrderAddDto Order { get; set; }
        public required List<OrderDetailReq> OrderDetails { get; set; }
    }
}
