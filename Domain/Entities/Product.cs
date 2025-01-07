using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required]
        public required DateTime UpdatedAt { get; set; }
        [Required]
        public required string Color { get; set; }
        [Required]
        public required Guid CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<Rating>? Ratings { get; set; }
        public List<ProductImage>? ProductImages { get; set; }
    }
}
