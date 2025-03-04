using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class AllProductsDto
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public List<AllProductsColorImageDto> ColorsAImages { get; set; }
        public int Sale { get; set; }
        public int Rating { get; set; }
        public Decimal Price { get; set; }
    }
}
