using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.Product
{
    public class GetProductsByCategoryReq
    {
        public string? CategoryName { get; set; }
        public string? BrandName { get; set; }
    }
}
