using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.Order
{
    public class OrderStatusHistoryDto
    {
        public required String Status { get; set; }
        public String? Note { get; set; }
        public DateTime UpdatedAt {  get; set; }
        public Guid OrderId { get; set; }
    }
}
