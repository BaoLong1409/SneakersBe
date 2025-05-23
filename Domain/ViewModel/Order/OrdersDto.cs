﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.Order
{
    public class OrdersDto
    {
        public Guid OrderId { get; set; }
        public Decimal TotalMoney { get; set; }
        public DateTime OrderDate { get; set; }
        public required string FirstProductName { get; set; }
        public required string ImageUrl { get; set; }
        public required string OrderStatus { get; set; }
    }
}
