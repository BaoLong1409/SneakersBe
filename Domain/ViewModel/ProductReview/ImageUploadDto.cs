using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.ProductReview
{
    public class ImageUploadDto
    {
        public required string FileName { get; set; }
        public required string Base64Data { get; set; }
    }
}
