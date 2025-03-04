using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class AllProductsColorImageDto
    {
        public Guid ColorId { get; set; }
        public Guid ImageId { get; set; }
        public required String ColorName { get; set; }
        public required String ThumbnailUrl { get; set; }
    }
}
