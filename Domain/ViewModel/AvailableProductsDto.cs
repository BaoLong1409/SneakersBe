using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class AvailableProductsDto
    {
        public Guid SizeId { get; set; }
        public float SizeNumber { get; set; }
        public int Quantity { get; set; }
    }
}
