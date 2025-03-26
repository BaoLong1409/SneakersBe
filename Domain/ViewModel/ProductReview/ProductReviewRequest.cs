using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.ProductReview
{
    public class ProductReviewRequest
    {
        public string? CommentContent { get; set; }
        public int Quality { get; set; }
        public ImageUploadDto[]? Image { get; set; }
        public required DateTime CreatedAt { get; set; } = DateTime.Now;
        public required DateTime UpdatedAt { get; set; } = DateTime.Now;

        public required Guid UserId { get; set; }
        public required Guid OrderDetailId { get; set; }
    }
}
