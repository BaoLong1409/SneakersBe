using Domain.Entities;
using Domain.ViewModel.ProductReview;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.Product
{
    public class UploadNewProductRequest
    {
        public required string ProductName { get; set; }

        [Range(0, 100, ErrorMessage = "Sale must be between 0 and 100")]
        public int Sale { get; set; } = 0;
        public Decimal Price { get; set; }
        public required Color Color { get; set; }
        public required List<string> CategoryName { get; set; }
        public required string Brand { get; set; }
        public required List<ImageUploadDto> ProductImages { get; set; }
        public int OrdinalImageThumbnail {  get; set; }
        public required List<UploadSizeRequest> SizesQuantity { get; set; }
    }
}
