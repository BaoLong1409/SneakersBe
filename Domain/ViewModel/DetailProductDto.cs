using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class DetailProductDto
    {
        public Guid Id { get; set; }
        public required String Name { get; set; }
        public String? Description { get; set; }
        public Decimal Price { get; set; }
        public int Sale {  get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ProductImageDto>? ProductImages { get; set; }
    }
}
