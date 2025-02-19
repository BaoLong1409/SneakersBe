using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductColor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required Guid Id { get; set; }
        [Required]
        public required Guid ColorId { get; set; }
        [Required]
        public required Guid ProductId { get; set; }
        public Color? Color { get; set; }
        public Product? Product { get; set; }
    }
}
