using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductReview
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string? CommentContent { get; set; }
        [Required]
        public int Quality { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [Required]
        public required Guid UserId { get; set; }
        public User? User { get; set; }
        [Required]
        public required Guid OrderDetailId { get; set; }
        public OrderDetail? OrderDetail { get; set; }
        public List<ProductReviewImage>? Images { get; set; }
    }
}
