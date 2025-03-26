using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.ProductReview
{
    public class ProductReviewDto
    {
        public Guid Id { get; set; }
        public string? CommentContent { get; set; }
        public int Quality { get; set; }
        public DateTime UpdatedAt { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required List<string> ImageUrl { get; set; }
    }
}
