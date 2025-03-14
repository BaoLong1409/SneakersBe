using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.Order
{
    public class OrderDetailReq
    {
        public required int Quantity { get; set; }
        public Decimal PriceAtOrder { get; set; }
        public required Guid ProductId { get; set; }
        public Guid ColorId { get; set; }
        public Guid SizeId { get; set; }
    }
}
