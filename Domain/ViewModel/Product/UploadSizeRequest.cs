using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.Product
{
    public class UploadSizeRequest
    {
        public Guid Id { get; set; }
        public float SizeNumber { get; set; }
        public int Quantity { get; set; }
    }
}
