using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.ProductReview
{
    public class ProductsAreWaitingReviewDto
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public Decimal PriceAtOrder { get; set; }
        public required string ColorName { get; set; }
        public float SizeNumber { get; set; }
        public required string Status {  get; set; }
        public required string ProductName { get; set; }
        public required string ImageUrl { get; set; }
    }
}
