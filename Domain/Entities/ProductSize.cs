using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductSize
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required Guid Id { get; set; }
        [Required]
        public required Guid SizeId { get; set; }
        public Size? Size { get; set; }
        [Required]
        public required Guid ProductId { get; set; }
        public Product? Product { get; set; }
        [Required]
        public required int StockQuantity { get; set; }
    }
}
