using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class ProductInCartDto
    {
        public Guid ProductId { get; set; }
        public String Name { get; set; }
        public Decimal Price { get; set; }
        public String ImageUrl { get; set; }
        public Guid ColorId { get; set; }
        public String ColorName { get; set; }
        public Guid SizeId { get; set; }
        public float SizeNumber { get; set; }
        public int Quantity { get; set; }

    }
}
