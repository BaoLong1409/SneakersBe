using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class ProductImageDto
    {
        public required String ImageUrl { get; set; }
        public int IsThumbnail { get; set; }
    }
}
