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
        public required string ProductName { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required] 
        public required Decimal Price { get; set; }
        [Required]
        public required int Sale { get; set; }

        [Required]
        public required DateTime UpdatedAt { get; set; }
        public List<ProductImage>? ProductImages { get; set; }
    }
}
