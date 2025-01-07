using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductPrice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public required Decimal Price { get; set; }
        [Required]
        public required int Sale { get; set; }
        [Required]
        public required string CurrencyCode { get; set; }
        public Product? Product { get; set; }
    }
}
